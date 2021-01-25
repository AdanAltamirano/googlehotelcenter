Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports MiscComponent
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.Facade

Public Class clsGetAvail

    Public Shared Function GetExeptionPrice(ByVal fecha As Date, ByVal exc As String) As String
        If exc <> "" Then
            If ApplyWeek(fecha, exc) Then
                Return "Y"
            Else
                Return "N"
            End If
        Else
            Return "N"
        End If
    End Function

    Private Function getDataAvail(ByVal idhotel As Integer, ByVal checkin As Date, ByVal checkout As Date, ByVal ratecode As String) As DataSet
        Dim conn As New SqlConnection(AppSettings("HotelConnection"))
        Dim idAsoc As Integer = GetIdAsociation()
        Dim str As String = If(idAsoc > 0, ",@idAsociacion=" + idAsoc.ToString, "")
        Dim da As New SqlDataAdapter(" Exec spBookingHotelGetAvailByDates " & idhotel & ",'" & checkin.ToString("yyyy/MM/dd") & _
            "','" & checkout.ToString("yyyy/MM/dd") & "'" & str, conn)
        Dim ds As New DataSet
        Try
            da.SelectCommand.CommandTimeout = 60
            da.Fill(ds)
        Catch ex As Exception

        End Try
        Return ds
    End Function

    '//////// Tipo de Rates ////////
    '//////// O - Open //////////
    '//////// C - Close /////////
    '//////// N - No Arrivals /////////
    '//////// NR - No Rates /////////
    '//////// NA - No Availability /////////
    Public Function GetAvail(ByVal checkin As Date, ByVal checkout As Date, _
        ByVal idhotel As Integer, ByVal ratecode As String) As DataTable

        Dim MinStay As Byte = 1, Maxstay As Byte = 99, AdvBook As Byte = 0
        Dim NoArrivos As String = "NNNNNNN"
        Dim StatusAvail As String = "O"
        Dim OnRequest As String = "N"
        Dim Nights As Integer = DateDiff(DateInterval.Day, checkin, checkout)
        Dim drhotel As DataRow
        'Dim dr As DataRow, drs() As DataRow
        Dim drsGral() As DataRow
        Dim dt As New DataTable

        Dim et As String = "O"

        dt.Columns.Add("fecha", GetType(Date))
        dt.Columns.Add("Avail", GetType(String))
        dt.Columns.Add("MaxStay", GetType(Byte))
        dt.Columns.Add("MinStay", GetType(Byte))
        dt.Columns.Add("AdvBook", GetType(Byte))
        dt.Columns.Add("Adult1", GetType(Double))
        dt.Columns.Add("Adult2", GetType(Double))
        dt.Columns.Add("idtipohabitacion_hotel", GetType(Integer))
        dt.Columns.Add("idrateplan", GetType(String))
        dt.Columns.Add("RateCode", GetType(String))
        dt.Columns.Add("Rooms", GetType(Integer))
        dt.Columns.Add("Res", GetType(Integer))

        dt.Columns.Add("GDS", GetType(Boolean))
        dt.Columns.Add("POR", GetType(Boolean))
        dt.Columns.Add("UNI", GetType(Boolean))

        dt.Columns.Add("Segment", GetType(String))

        Dim ds As DataSet

        'With New WSHotelDataAccess.clsDAHOC
        ds = getDataAvail(idhotel, checkin, checkout, ratecode) '.GetComAvail("," & idhotel & ",", checkin, checkout, 2, 0, "")
        '        End With

        For i As Integer = 0 To Nights
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then

                drhotel = ds.Tables(0).Rows(0)

                et = "O"
                MinStay = 1
                Maxstay = 99
                AdvBook = 0
                StatusAvail = "O"
                NoArrivos = "NNNNNNN"

                drsGral = ds.Tables(2).Select("fecha=#" & checkin.AddDays(i).ToString("M/dd/yy") & "#")
                GetRestricted(drhotel, MinStay, Maxstay, AdvBook, StatusAvail, NoArrivos, checkin.AddDays(i))
                If NoArrivos <> "" AndAlso ApplyWeek(checkin.AddDays(i), NoArrivos, "Y") Then
                    StatusAvail = "N"
                End If

                If drsGral.Length > 0 Then
                    If StatusAvail <> "C" AndAlso StatusAvail <> "N" Then
                        GetRestricted(drsGral(0), MinStay, Maxstay, AdvBook, StatusAvail, NoArrivos, checkin.AddDays(i), "LG")
                    Else
                        GetRestricted(drsGral(0), MinStay, Maxstay, AdvBook, "", NoArrivos, checkin.AddDays(i), "LG")
                    End If
                End If

                Call ValidateRestricted(StatusAvail, checkin.AddDays(i), "NNNNNNN", et) 'Then
                ApplyRulesRatesRoom(ds, dt, drsGral, MinStay, Maxstay, AdvBook, StatusAvail, checkin.AddDays(i), et, ratecode)

            End If
        Next

        Return dt
    End Function




    Private Function getRateLinked(ByVal ds As DataSet, ByVal dr As DataRow, _
         ByVal RateCode As String, ByVal idtipohab As Integer) As DataRow

        Dim drtemp As DataRow
        Dim drs() As DataRow
        Dim sRP As String
        Dim sTH As Integer
        sRP = RateCode
        sTH = idtipohab
        If Not dr.IsNull("SourceRatePlan") Then
            sRP = dr("SourceRatePlan")
        End If
        If Not dr.IsNull("IdTipohabitacion_Source") Then
            sTH = dr("IdTipohabitacion_Source")
        End If
        drs = ds.Tables(1).Select("codigotarifa ='" & sRP & "' and idtipohabitacion_hotel=" & sTH)
        If drs.Length > 0 Then
            drtemp = drs(0)
        Else
            drtemp = dr
        End If
        Return drtemp

    End Function

    '' Autor: Javier Adrián Ramírez Ayala
    '' aplica las reglas de las habitacion Rate Plan
    Private Sub ApplyRulesRatesRoom(ByRef ds As DataSet, ByRef dt As DataTable, ByVal drsGral As DataRow(), ByRef minStay As Byte, ByVal MaxStay As Byte, ByVal AdvBook As Byte, _
        ByVal StatusAvail As String, ByVal checkin As Date, ByRef et As String, ByVal rateCode As String)
        Dim drRateRooms(), dr As DataRow
        Dim f As Date
        'Dim i As Integer
        'Dim strMessage As String
        Dim noarrivos As String = "NNNNNNN"
        'Dim drs() As DataRow
        If rateCode <> "" Then
            drRateRooms = ds.Tables(1).Select("codigotarifa='" & rateCode & "'")
        Else
            drRateRooms = ds.Tables(1).Select()
        End If

        Dim minS As Byte, MaxS As Byte, AdvB As Byte, noarrivosS As String = ""
        Dim StatusA As String
        Dim qtyRooms As Integer
        Dim Porc As Single
        Dim drtemp As DataRow

        Dim price1 As Double
        Dim price2 As Double
        Dim hab As Integer
        Dim res As Integer
        Dim drt As DataRow
        Dim drsRule() As DataRow
        Dim et2 As String

        For Each dr In drRateRooms
            hab = 0
            res = 0
            price1 = 0
            price2 = 0
            et2 = et
            'dr("onrequest") = onrequest
            If Not dr.IsNull("RateRulesDefault") AndAlso Not dr("RateRulesDefault") Then
                minS = 1
                MaxS = Byte.MaxValue
                AdvB = 0
                noarrivosS = ""
            Else
                minS = minStay
                MaxS = MaxStay
                AdvB = AdvBook
                noarrivosS = noarrivos
            End If
            StatusA = StatusAvail
            '//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '//'''''''''' leo la configuracion inicial de los planes'//''''''''''''''''''''
            GetRestricted(dr, minS, MaxS, AdvB, StatusA, noarrivosS, checkin)
            '//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '//la configuracion  de los bloqueos y configuracion de lo s planes por fechas
            drsRule = ds.Tables(3).Select("fecha='" & checkin.ToString("yyyy/MM/dd") & "' and codigotarifa='" & dr("CodigoTarifa") & "'")
            If drsRule.Length > 0 Then
                GetRestricted(drsRule(0), minS, MaxS, AdvB, StatusA, noarrivosS, checkin, "lrp")
            End If
            '//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '//'''''''''' leo la configuracion de las tarifas '''''''''''''''''''''''''''''''''
            drtemp = getRateLinked(ds, dr, dr("CodigoTarifa"), dr("idtipohabitacion_hotel"))
            drsRule = ds.Tables(5).Select("fechainicia<=#" & checkin.ToString("M/dd/yy") & "# and RateCode='" & drtemp("RateCode") & "'" _
                & "and fechaFinaliza>=#" & checkin.ToString("M/dd/yy") & "#")
            If drsRule.Length > 0 Then
                If Not drsRule(0).IsNull("tRateRulesDefault") AndAlso Not drsRule(0)("tRateRulesDefault") Then
                    minS = 1
                    MaxS = Byte.MaxValue
                    AdvB = 0
                    noarrivosS = ""
                End If
                GetRestricted(drsRule(0), minS, MaxS, AdvB, StatusA, noarrivosS, checkin, "T")
            End If
            '//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            f = checkin
            If Not ValidateRestricted(checkin, noarrivosS, et2) Then
                'Exit For
            End If
            '//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            qtyRooms = -1
            Porc = -1
            If Not dr.IsNull("QtyRooms") Then
                qtyRooms = dr("QtyRooms")
            End If
            If Not dr.IsNull("VQtyRooms") Then
                qtyRooms = dr("VQtyRooms")
            End If


            ApplyRulesRatesSeasson(ds, dr("RateCode"), minS, MaxS, AdvB, StatusA, checkin _
            , noarrivosS, dr("codigotarifa"), dr("RoomCode"), dr("idtipohabitacion_hotel"), qtyRooms, Porc, _
            dr("RateCodeV"), dr("idtipohabvinc"), et2, price1, price2, hab, res, dr)

            If drsGral.Length > 0 Then
                GetRestrictedHotel(drsGral(0), minS, MaxS, checkin, "LG")
            End If

            drt = dt.NewRow
            drt("fecha") = checkin
            drt("Avail") = et2 'StatusAvail
            drt("MaxStay") = MaxS
            drt("MinStay") = minS
            drt("AdvBook") = AdvB
            drt("Adult1") = price1
            drt("Adult2") = price2
            drt("idtipohabitacion_hotel") = dr("idtipohabitacion_hotel")
            drt("idrateplan") = dr("CodigoTarifa")
            drt("RateCode") = dr("RateCode")
            drt("Rooms") = hab
            drt("Res") = res

            drt("GDS") = dr("RateGDS")
            drt("POR") = dr("RateUnip")
            drt("UNI") = dr("RatePortal")

            drt("Segment") = dr("Segment")

            dt.Rows.Add(drt)
        Next

    End Sub

    '' Autor: Javier Adrián Ramírez Ayala
    '' aplica las reglas de las habitacion plans
    Private Sub ApplyRulesRatesSeasson(ByRef ds As DataSet, ByVal RateCode As String, _
        ByRef minStay As Byte, ByRef MaxStay As Byte, ByRef AdvBook As Byte, ByRef StatusAvail As String, ByVal checkin As Date _
        , ByRef noarrivos As String, _
        ByVal codigotarifa As String, ByVal CodigoHab As String, ByVal idtipohab As Integer, ByVal QtyRooms As Integer, _
        ByVal soldOutPerc As Single, ByVal ratecodeV As String, ByVal idtipohabvinc As Integer, ByRef et As String, ByRef price1 As Double, _
        ByRef price2 As Double, ByRef Hab As Integer, ByRef res As Integer, ByVal drType As DataRow)

        Dim drRateSeasson() As DataRow, dr As DataRow
        Dim drPrices() As DataRow

        Dim drLockRatePlan() As DataRow
        Dim drLockRatePlanRoom() As DataRow

        drRateSeasson = ds.Tables(2).Select("idtipohabitacion_hotel=" & idtipohab & " and fecha=#" & checkin.ToString("M/dd/yy") & "#")

        'Dim noarrivos As String = "NNNNNNN"
        'Dim strmessage As String

        'Dim minS As Byte, MaxS As Byte, AdvB As Byte, 
        Dim noarrivosS As String = ""
        Dim Vez1 As Boolean = True

        'Dim LRdr As DataRow
        'Dim LRdrs() As DataRow

        'Dim drRat As DataRow

        Dim exc As String = "NNNNNNN"
        Dim strexc As String = ""
        Dim drp As DataRow
        If drRateSeasson.Length > 0 Then
            dr = drRateSeasson(0)
            strexc = ""
            drPrices = ds.Tables(5).Select("RateCode='" & ratecodeV & "' and idtipohabitacion_hotel=" & idtipohabvinc & _
             " and fechainicia<=#" & checkin.ToString("M/dd/yy") & "# and fechafinaliza>= #" & checkin.ToString("M/dd/yy") & "#")


            If drPrices.Length = 0 Then



                If et = "O" Then et = "NR" 'oAvailability 
                Exit Sub
            End If

            If GetExeptionPrice(dr("Fecha"), exc) = "Y" Then
                strexc = "exc"
            End If
            Dim ratio As Double
            Dim offset As Double
            For Each drp In drPrices
                If drp("Adultos") = 1 And drp("ninios") = 0 Then
                    price1 = drp("tarifaadulto" & strexc)
                    '//'''//////////                    
                    If Not drType.IsNull("SourceRatePlan") Then
                        If drType.IsNull("OnePersonRatio") OrElse drType("OnePersonRatio") = 0 Then
                            ratio = 1
                        Else
                            ratio = drType("OnePersonRatio")
                        End If
                        offset = 0
                        If Not drType.IsNull("OnePersonOffset") Then
                            offset = drType("OnePersonOffset")
                        End If
                        price1 = price1 * ratio + offset
                        'tarAd = tarAd * ratio + offset
                        'tarAdExc = tarAdExc * ratio + offset
                    End If
                    '--------------------------- link roomtypes ----------------
                    If Not drType.IsNull("IdTipohabitacion_Source") Then
                        ',lrt.OnePersonRatio,lrt.OnePersonOffset,lrt.TwoPersonRatio,lrt.TwoPersonOffset,lrt.ExtraAdultRatio,lrt.ExtraAdultOffset
                        ',lrt.ExtraChildRatio,lrt.ExtraChildOffset,lrt.OtherOccupationRatio,lrt.OtherOccupationOffset,
                        'lrt.IdTipohabitacion_Source,
                        If drType.IsNull("tOnePersonRatio") OrElse drType("tOnePersonRatio") = 0 Then
                            ratio = 1
                        Else
                            ratio = drType("tOnePersonRatio")
                        End If
                        offset = 0
                        If Not drType.IsNull("tOnePersonOffset") Then
                            offset = drType("tOnePersonOffset")
                        End If
                        price1 = price1 * ratio + offset
                        'tarAdExc = tarAdExc * ratio + offset

                    End If
                    If price1 < 0 Then
                        price1 = 0
                    End If
                    '//''''''//////////////////////////////
                End If
                If drp("Adultos") = 2 And drp("ninios") = 0 Then
                    price2 = drp("tarifaadulto" & strexc)
                    If Not drType.IsNull("SourceRatePlan") Then
                        If drType.IsNull("TwoPersonRatio") OrElse drType("TwoPersonRatio") = 0 Then
                            ratio = 1
                        Else
                            ratio = drType("TwoPersonRatio")
                        End If
                        offset = 0
                        If Not drType.IsNull("TwoPersonOffset") Then
                            offset = drType("TwoPersonOffset")
                        End If
                        price2 = price2 * ratio + offset

                    End If

                    If Not drType.IsNull("IdTipohabitacion_Source") Then
                        If drType.IsNull("tTwoPersonRatio") OrElse drType("tTwoPersonRatio") = 0 Then
                            ratio = 1
                        Else
                            ratio = drType("tTwoPersonRatio")
                        End If
                        offset = 0
                        If Not drType.IsNull("tTwoPersonOffset") Then
                            offset = drType("tTwoPersonOffset")
                        End If
                        price2 = price2 * ratio + offset
                        'tarAdExc = tarAdExc * ratio + offset
                    End If
                    If price2 < 0 Then
                        price2 = 0
                    End If

                End If
            Next
            exc = drPrices(0)("Excepciones")



            drLockRatePlan = ds.Tables(3).Select("codigotarifa='" & codigotarifa & "'" & _
                " and fecha ='" & CDate(dr("fecha")).ToString("yyyy/MM/dd") & "'")
            '//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            drLockRatePlanRoom = ds.Tables(4).Select("idtipohabitacion_hotel=" & idtipohab & "  and codigotarifa='" & codigotarifa & "'" & _
            " and fecha =#" & checkin.ToString("M/dd/yy") & "#")
            '//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '//'''''''''''''''''''''Javier Adrian Ramirez'''''''''''''''''''''''''''
            '//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '' cheka las restricciones Generales ''

            GetRestricted(dr, StatusAvail, dr("fecha"), "LG")
            If Not ValidateRestricted(StatusAvail, Now.AddDays(-1), "", et) Then

            ElseIf drLockRatePlan.Length > 0 Then
                GetRestricted(drLockRatePlan(0), StatusAvail, dr("fecha"), "lrp")
                If Not ValidateRestricted(StatusAvail, Now.AddDays(-1), "", et) Then

                End If
            End If
            '//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Hab = dr("dispo")
            res = dr("ReservasRoom")



            If drLockRatePlanRoom.Length > 0 Then 'If Not dr.IsNull("RoomsAvailable") Then                '                
                If drLockRatePlanRoom(0)("RoomsAvailable") - drLockRatePlanRoom(0)("Reservas") - 1 < 0 Then
                    Hab = 0
                    res = drLockRatePlanRoom(0)("Reservas")
                    If et = "O" Then et = "NA"
                Else
                    '//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    If drLockRatePlanRoom(0)("RoomsAvailable") - drLockRatePlanRoom(0)("Reservas") < Hab Then
                        Hab = drLockRatePlanRoom(0)("RoomsAvailable") - drLockRatePlanRoom(0)("Reservas")
                        res = drLockRatePlanRoom(0)("Reservas")
                    End If
                    If drLockRatePlan.Length > 0 Then
                        If Not ValidateRoomsRates(drLockRatePlan(0), ds, 1, 0, 0, QtyRooms, soldOutPerc, Hab, res) Then
                            If et = "O" Then et = "NA"




                        End If
                    Else
                        If Not ValidateRoomsRates(Nothing, ds, 1, 0, 0, QtyRooms, soldOutPerc, Hab, res) Then
                            If et = "O" Then et = "NA" 'strmessage = GetErrorMessage(ErrorType.NoAvailability)
                        End If




                    End If
                End If                   '//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '//''''''''''''''''''''' Rooms Available ''''''''''''''''''''''''''''''''
                'If drLockRatePlan.Length > 0 AndAlso Not drLockRatePlan(0).IsNull("ReservasRate") _
                '    AndAlso drLockRatePlanRoom(0)("RoomsAvailable") - drLockRatePlan(0)("ReservasRate") < Hab Then
                '    Hab = drLockRatePlanRoom(0)("RoomsAvailable") - drLockRatePlan(0)("ReservasRate")
                'End If
                '//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Else
                If dr("dispo") - 1 < 0 Then
                    If et = "O" Then et = "NA" 'noavailability
                Else
                    '/ para que solo valide roomRatesPlus
                    If drLockRatePlan.Length > 0 Then
                        If Not ValidateRoomsRates(drLockRatePlan(0), ds, 1, 0, 0, QtyRooms, soldOutPerc, Hab, res) Then
                            If et = "O" Then et = "NA"
                        End If
                    Else
                        If Not ValidateRoomsRates(Nothing, ds, 1, 0, 0, QtyRooms, soldOutPerc, Hab, res) Then
                            If et = "O" Then et = "NA"
                        End If
                    End If
                End If
            End If



        End If

        If drRateSeasson.Length = 0 AndAlso et = "O" Then
            drPrices = ds.Tables(5).Select("RateCode='" & ratecodeV & "' and idtipohabitacion_hotel=" & idtipohabvinc & _
             " and fechainicia<=#" & checkin.ToString("M/dd/yy") & "# and fechafinaliza>= #" & checkin.ToString("M/dd/yy") & "#")
            If drPrices.Length = 0 Then
                et = "NR"
            Else
                et = "NA"
            End If

        End If

    End Sub

    Private Function isnullRules(ByVal dr As DataRow, ByVal pref As String) As Boolean
        If dr.IsNull(pref & "minstay") AndAlso dr.IsNull(pref & "maxstay") AndAlso dr.IsNull(pref & "advbooking") AndAlso _
            dr.IsNull(pref & "StatusAvailability") AndAlso dr.IsNull(pref & "applyweek") Then
            Return True
        End If
        Return False
    End Function

    '// valida si las si hay disponibilidad con los lockroomsrates
    Private Function ValidateRoomsRates(ByVal dr As DataRow, ByRef ds As DataSet, ByVal Rooms As Byte, ByVal roomres As Byte, ByVal roomRatesplus As Integer, _
        ByVal QtyRooms As Integer, ByVal SoldOutPerc As Single, ByRef hab As Integer, ByRef res As Integer) As Boolean
        Dim porc As Single = 100
        Dim soldout As Single = 0
        'Dim drs() As DataRow
        Dim dispo As Integer
        Dim reservasRate As Integer
        If QtyRooms >= 0 Then
            If dr Is Nothing OrElse dr.IsNull("dispo") Then
                dispo = QtyRooms
                reservasRate = 0
            Else
                dispo = dr("dispo")
                reservasRate = dr("reservasrate")
            End If
            If dispo + roomRatesplus - Rooms < 0 Then
                Return False
            Else
                If hab > dispo Then
                    hab = dispo
                    res = reservasRate
                End If
            End If
            If SoldOutPerc >= 0 Then
                porc = SoldOutPerc
                soldout = CInt(QtyRooms * porc / 100)
                If soldout + roomRatesplus - reservasRate - Rooms < 0 Then
                    Return False
                Else
                    If dispo > soldout - reservasRate Then
                        hab = soldout - reservasRate
                        res = reservasRate
                    End If
                End If
            End If
        End If
        Return True
    End Function

    Private Function ValidateRestricted(ByRef StatusAvail As String, ByVal checkin As Date, ByVal NoArrivals As String, ByRef ErrType As String) As Boolean
        If ErrType = "O" Then
            If StatusAvail <> "" AndAlso StatusAvail <> "O" Then
                '' status availability si esta cerrado u abierto el hotel
                If StatusAvail = "N" Then
                    ErrType = "N"
                Else
                    ErrType = "C"
                End If
                Return False
            End If
            If NoArrivals <> "" AndAlso ApplyWeek(checkin, NoArrivals, "Y") Then
                ErrType = "N"
                Return False
            End If
        End If
        Return True
    End Function

    Public Shared Function ApplyWeek(ByVal f As Date, ByVal cadSem As String, Optional ByVal YN As String = "Y") As Boolean
        If Not cadSem Is Nothing AndAlso cadSem.Length >= 7 Then
            If f.DayOfWeek = DayOfWeek.Sunday Then
                If cadSem.Substring(6, 1).ToUpper = YN Then
                    Return True
                Else
                    Return False
                End If
            Else
                If cadSem.Substring(f.DayOfWeek - 1, 1).ToUpper = YN Then
                    Return True
                Else
                    Return False
                End If
            End If
        End If
    End Function

    '' Autor: Javier Adrian Ramirez
    '' valida las restricciones de las tarifas del hotel
    Private Function ValidateRestricted(ByVal checkin As Date, ByVal NoArrivals As String, ByRef ErrType As String) As Boolean
        If ErrType = "O" Then
            If NoArrivals <> "" AndAlso ApplyWeek(checkin, NoArrivals, "Y") Then
                '//'''''''''' No Arrival ''''''''''''''''''''''''''''''
                ErrType = "N"
                Return False
            End If
        End If
        Return True

    End Function

    '' Autor Javier Adrian Ramirez Ayala
    '' se va obtener las restricciones de los datarows, el prefijo es por quien venga..
    Private Sub GetRestricted(ByVal dr As DataRow, ByRef minStay As Byte, ByRef MaxStay As Byte, ByRef AdvBook As Byte, ByRef StatusAvail As String, ByRef NoArrivals As String, ByVal Fecha As Date, Optional ByVal pref As String = "")
        '//'''' Apply Week ''''' 
        Dim AppWeek As String = "YYYYYYY"
        If Not dr.Table.Columns(pref & "ApplyWeek") Is Nothing AndAlso Not dr.IsNull(pref & "ApplyWeek") AndAlso dr(pref & "ApplyWeek") <> "" Then
            AppWeek = dr(pref & "ApplyWeek")
        End If
        If ApplyWeek(Fecha, AppWeek, "Y") Or dr.Table.Columns(pref & "ApplyWeek") Is Nothing Then
            '//' min stay '''
            If Not dr.IsNull(pref & "minstay") AndAlso dr(pref & "minstay") > 0 Then
                minStay = dr(pref & "minstay")
                If minStay < 0 Or minStay > 99 Then
                    minStay = 0
                End If
            End If
            '//' max stay '''
            If Not dr.IsNull(pref & "maxstay") AndAlso dr(pref & "maxstay") < 99 Then
                MaxStay = dr(pref & "maxstay")
                If MaxStay <= 0 Then
                    MaxStay = 99
                End If
            End If
            '//'''' adv book '''''
            If Not dr.IsNull(pref & "AdvBooking") AndAlso dr(pref & "AdvBooking") > 0 Then
                AdvBook = dr(pref & "advBooking")
                If AdvBook < 0 Then
                    AdvBook = 0
                End If
            End If
            '//'''' status availability ''''' si es que tiene
            If Not dr.Table.Columns(pref & "StatusAvailability") Is Nothing AndAlso Not dr.IsNull(pref & "StatusAvailability") AndAlso dr(pref & "StatusAvailability").ToString().Trim() <> "" Then
                StatusAvail = dr(pref & "StatusAvailability")
            End If
        End If
        '//'''' Para los no arrivos ''''' si es que tiene
        If Not dr.Table.Columns(pref & "NoArrivos") Is Nothing AndAlso Not dr.IsNull(pref & "NoArrivos") AndAlso dr(pref & "NoArrivos") <> "" Then
            If Not dr.Table.Columns(pref & "RateRulesDefault") Is Nothing AndAlso Not dr.IsNull(pref & "RateRulesDefault") AndAlso dr(pref & "RateRulesDefault") AndAlso dr(pref & "NoArrivos") = "NNNNNNN" Then
            Else
                NoArrivals = dr(pref & "NoArrivos")
            End If
        End If
    End Sub

    Private Sub GetRestrictedHotel(ByVal dr As DataRow, ByRef minStay As Byte, ByRef MaxStay As Byte, ByVal Fecha As Date, Optional ByVal pref As String = "")
        '//'''' Apply Week ''''' 
        Dim AppWeek As String = "YYYYYYY"
        If Not dr.Table.Columns(pref & "ApplyWeek") Is Nothing AndAlso Not dr.IsNull(pref & "ApplyWeek") AndAlso dr(pref & "ApplyWeek") <> "" Then
            AppWeek = dr(pref & "ApplyWeek")
        End If
        If ApplyWeek(Fecha, AppWeek, "Y") Or dr.Table.Columns(pref & "ApplyWeek") Is Nothing Then
            '//' min stay '''
            If Not dr.IsNull(pref & "minstay") AndAlso dr(pref & "minstay") > 0 Then
                minStay = dr(pref & "minstay")
                If minStay < 0 Or minStay > 99 Then
                    minStay = 0
                End If
            End If
            '//' max stay '''
            If Not dr.IsNull(pref & "maxstay") AndAlso dr(pref & "maxstay") < 99 Then
                MaxStay = dr(pref & "maxstay")
                If MaxStay <= 0 Then
                    MaxStay = 99
                End If
            End If
        End If
     
    End Sub

    '' Autor Javier Adrian Ramirez Ayala
    '' se va obtener las restricciones de los datarows, el prefijo es por quien venga..
    Private Sub GetRestricted(ByVal dr As DataRow, ByRef StatusAvail As String, ByVal Fecha As Date, Optional ByVal pref As String = "")
        '//''''''''' Apply Week ''''' ''''''''
        Dim AppWeek As String = "YYYYYYY"
        If Not dr.Table.Columns(pref & "ApplyWeek") Is Nothing AndAlso Not dr.IsNull(pref & "ApplyWeek") AndAlso dr(pref & "ApplyWeek") <> "" Then
            AppWeek = dr(pref & "ApplyWeek")
        End If
        If ApplyWeek(Fecha, AppWeek, "Y") Or dr.Table.Columns(pref & "ApplyWeek") Is Nothing Then
            '//'''' status availability ''''' si es que tiene
            If Not dr.Table.Columns(pref & "StatusAvailability") Is Nothing AndAlso Not dr.IsNull(pref & "StatusAvailability") AndAlso dr(pref & "StatusAvailability").ToString().Trim() <> "" Then
                StatusAvail = dr(pref & "StatusAvailability")
            End If
        End If
    End Sub

    '' Autor: Adrián Ramírez Ayala ...
    '' se va grabar el mensaje de los renglones que se quiere indicar no disponibilidad ...
    Private Sub setMessage(ByRef drows() As DataRow, ByVal msg As String)
        For Each dr As DataRow In drows
            dr("Message") = msg
            dr("Availability") = "N"
        Next
    End Sub
    '' Autor: Adrián Ramírez Ayala ...
    '' funcion que regresa la ocupacion de la reservacion y si no exede el numero de personas y extras ...
    Private Function CheckOccupation(ByVal Rooms As Byte, ByVal Adults As String, ByVal Children As String, ByVal MaxAdults As Byte, ByVal MaxChildren As Byte, ByVal Extras As Byte) As Boolean
        Dim i As Integer
        Dim Ad, Ch As Integer
        Dim ExtCh, ExtAd As Integer
        For i = 1 To Rooms
            Ad = CInt(Adults.Replace(",,", ",").Split(",")(i))
            Ch = CInt(Children.Replace(",,", ",").Split(",")(i))
            If CInt(Ch) - CInt(MaxChildren) > 0 Then
                ExtCh = CInt(Ch) - CInt(MaxChildren)
                Ch -= ExtCh
            End If
            If CInt(Ad) - CInt(MaxAdults) > 0 Then
                ExtAd = CInt(Ad) - CInt(MaxAdults)
                Ad -= ExtAd
            End If
            If ExtCh + ExtAd > Extras Then
                Return (False)
            End If
        Next
        Return True
    End Function

    Function GetIdAsociation()
        Dim IdAsociation As Integer = -1
        Try
            If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), IdAsociation)
            IdAsociation = If(IdAsociation = 0, -1, IdAsociation)
        Catch ex As Exception
        End Try
        Return IdAsociation
    End Function

    Public Function GenerateReportAvail(ByVal idhotel As Integer, ByVal HotelName As String, ByVal NoEmpresa As Integer, ByVal checkin As Date, ByVal UTDisp As String, ByVal dDisp As Integer) As String
        Dim dt As DataTable
        Dim diasDisp As Integer = dDisp
        Dim UnitTypeDisp As String = UTDisp
        'Dim diasGenerate As Integer = 1
        Dim UnitTypeGen As String = UTDisp
        Dim idAsoc As Integer = Me.GetIdAsociation
        'Dim Nights As Integer
        ' Dim drRatePlanDate() As DataRow

        Dim ckin As Date = checkin, ckout As Date

        Select Case UnitTypeDisp
            Case "D"
                ckout = ckin.AddDays(diasDisp)
            Case "W"
                ckout = ckin.AddDays(diasDisp * 7)
            Case "M"
                ckout = ckin.AddMonths(diasDisp)
            Case "Y"
                ckout = ckin.AddYears(diasDisp)
        End Select

        dt = GetAvail(ckin, ckout, idhotel, "")
        Dim arrRates As ArrayList = New ArrayList
        Dim dsrooms As RoomsHotelData
        With New RoomFacade
            dsrooms = .getRooms(idhotel, PortalCulture.GetIDCulture)
        End With

        Dim dsRates As RatePlanData

        With New RatePlanFacade
            dsRates = .GetRatePlanByIdHotel(idhotel, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)
        End With
        Dim cadSource As String
        For Each drRate As DataRow In dsRates.Tables(0).Rows
            For Each drRoom As DataRow In dsrooms.Tables(0).Rows
                cadSource = ""

                If drRate("rategds") Then
                    cadSource = "|G"
                Else
                    cadSource = "|"
                End If

                If drRate("RateUnip") Then
                    cadSource &= "|U"
                Else
                    cadSource &= "|"
                End If

                If drRate("RatePortal") Then
                    cadSource &= "|P"
                Else
                    cadSource &= "|"
                End If

                arrRates.Add(drRoom("CodigoHabitacion") & drRate("CodigoTarifa") & "&" & cadSource)
                'Me.SourceRateName &= "//" & ds.Tables(ds.RATEPLAN_TABLE).Rows(j).Item(ds.FIELD_NAME).ToString
                'lblDescRatePlan.Text = "" & ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0)(ds.FIELD_NAME)

            Next
        Next

        'If TypeOf dt Is Integer Then

        With New cExportExcel
            .asbly = System.Reflection.Assembly.GetExecutingAssembly()
            .ci = PortalCulture.GetCulture
            .BaseName = "RateManager.strings"
            Return .ExportExcel(dt, DateDiff(DateInterval.Day, ckin, ckout), ckin, HotelName, NoEmpresa, arrRates)
        End With
        'End If

    End Function



End Class

