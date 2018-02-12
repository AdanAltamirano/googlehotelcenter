Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.General.Common.Data
Imports Portal.General.Facade

Partial Class RateChart
    Inherits PaginaBase

    Private Property totalrooms() As Integer
        Get
            Return viewstate("_tr")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_tr") = Value
        End Set
    End Property
    Private Property DifDays() As Integer
        Get
            Return viewstate("_DifDays")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_DifDays") = Value
        End Set
    End Property

    Public Property SourceRateName() As String
        Get
            Return viewstate("_SRN")

        End Get
        Set(ByVal Value As String)
            viewstate("_SRN") = Value
        End Set
    End Property

    Enum dgcolumns
        Fecha
        Codigo
        oneperson
        twoperson
        Min
        Max
        Avail
    End Enum
#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            Me.txtDateFrom.Text = Date.Today.ToString("MM/dd/yyyy")
            Me.txtDateTo.Text = Date.Today.AddDays(12).ToString("MM/dd/yyyy")
            loaddatos()
            btnload_Click(Nothing, Nothing)
        End If
    End Sub

    Private Function RatePlanFilter(ByVal segmentType As String, ByVal RatesPlan As Portal.General.Common.Data.RatePlanData) As Portal.General.Common.Data.RatePlanData
        'Elimina los planes tarifarios que contengan el tipo de segmento especificado
        Dim dv2 As DataView
        For Each r As DataRow In RatesPlan.Tables("RatePlans").Rows()
            dv2 = RatesPlan.Tables("RatePlans").DefaultView
            dv2.RowFilter = "Segment" & "=" & "'" & segmentType & "'"
            If dv2.Count > 0 AndAlso r("Segment").ToString() = segmentType Then
                r.Delete()
            End If
        Next
        RatesPlan.AcceptChanges()
        Return RatesPlan
    End Function

    Private Sub loaddatos()
        Dim ds As RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation
        With New RatePlanFacade
            If MyBase.IsSupervisor Then
                ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 0, 1, idAsociacion:=idAsoc, DeleteFilter:=1)
            Else
                ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, idAsociacion:=idAsoc, DeleteFilter:=1)
            End If

        End With

        If MyBase.IdCorporativoUserChain = 4 AndAlso (MyBase.IsHotel Or MyBase.IsUsuarioHotel) Then
            ds = RatePlanFilter("C", ds)
        End If

        For j As Integer = 0 To ds.Tables(ds.RATEPLAN_TABLE).Rows.Count - 1
            Me.SourceRateName &= "//" & ds.Tables(ds.RATEPLAN_TABLE).Rows(j).Item(ds.FIELD_NAME).ToString
            lblDescRatePlan.Text = "" & ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0)(ds.FIELD_NAME)
        Next

        ddlrateplans.DataTextField = ds.FIELD_CODIGOTARIFA
        ddlrateplans.DataValueField = ds.FIELD_IDRATEPLAN
        ddlrateplans.DataSource = ds
        ddlrateplans.DataBind()

    End Sub

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

    '' Autor Javier Adrian Ramirez Ayala
    '' se va obtener las restricciones de los datarows, el prefijo es por quien venga..
    Private Sub GetRestricted(ByVal dr As DataRow, ByRef minStay As Byte, ByRef MaxStay As Byte, ByRef AdvBook As Byte, ByRef StatusAvail As String, ByRef NoArrivals As String, ByVal Fecha As Date, Optional ByVal pref As String = "")
        ''''''' Apply Week ''''' 
        Dim AppWeek As String = "YYYYYYY"
        Dim prefs() As String = pref.Split("|")
        If prefs.Length > 5 AndAlso Not dr.IsNull(prefs(5)) AndAlso dr(prefs(5)) <> "" Then
            AppWeek = dr(prefs(5))
        End If
        If ApplyWeek(Fecha, AppWeek, "Y") Or prefs.Length > 0 Then
            '''' min stay '''
            If Not dr.IsNull(prefs(0)) AndAlso dr(prefs(0)) > 0 Then
                minStay = dr(prefs(0))
                If minStay < 0 Or minStay > 99 Then
                    minStay = 0
                End If
            End If
            '''' max stay '''
            If Not dr.IsNull(prefs(1)) AndAlso dr(prefs(1)) < 99 Then
                MaxStay = dr(prefs(1))
                If MaxStay <= 0 Then
                    MaxStay = 99
                End If
            End If
            ''''''' adv book '''''
            If Not dr.IsNull(prefs(2)) AndAlso dr(prefs(2)) > 0 Then
                AdvBook = dr(prefs(2))
                If AdvBook < 0 Then
                    AdvBook = 0
                End If
            End If
            ''''''' status availability ''''' si es que tiene
            If prefs(3) <> "" AndAlso Not dr.IsNull(prefs(3)) AndAlso dr(prefs(3)) <> "" Then
                StatusAvail = dr(prefs(3))
            End If
        End If
        ''''''' Para los no arrivos ''''' si es que tiene
        If prefs.Length > 4 AndAlso Not dr.IsNull(prefs(4)) AndAlso dr(prefs(4)) <> "" Then
            If prefs.Length > 6 AndAlso Not dr.IsNull(prefs(6)) AndAlso dr(prefs(6)) AndAlso dr(prefs(4)) = "NNNNNNN" Then
            Else
                NoArrivals = dr(prefs(4))
            End If
        End If
    End Sub

    Function CreateDataSourceHorizontal() As ICollection
        Dim DS As DataSet, RP As RatePlanData, strError As String
        Dim dt As New DataTable, dr As DataRow, dr1 As DataRow, dr2 As DataRow, ratesplans As String = "", dr3 As DataRow, dr4 As DataRow, dr5 As DataRow
        Dim dsrooms As RoomsHotelData, data As DataSet ', Reservas As DataSet ,dsGral As clsCommonAvailibilityGral,

        Dim strColumns As String
        Dim checkin As Date
        Dim dtRates As DataTable
        checkin = CDate(txtDateFrom.Text)
        '////////////////////// carga datos disponibilidad ///////
        With New clsGetAvail
            dtRates = .GetAvail(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), MyBase.cInfoActual.Hotel, ddlrateplans.SelectedItem.Text)
        End With
        
        With New RoomFacade
            dsrooms = .getRooms(Me.cInfoActual.Hotel)
        End With
        
        '/////////////////////////////////////////////////////////
        dt.Columns.Add(New DataColumn("ROOM", GetType(String)))
        dt.Columns.Add(New DataColumn("IDROOM", GetType(String)))
        dt.Columns.Add(New DataColumn("DATA", GetType(String)))

        For i As Integer = 0 To DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))
            dt.Columns.Add(New DataColumn(CDate(txtDateFrom.Text).AddDays(i), GetType(String)))
        Next
        Dim dvRP, dvRes, dvgral, dvlocks As DataView
        Dim drRatePlanDate() As DataRow
        ''''''''''''''''por cada habitacion ''''''''''''''''''''''''''''''''''''
        For Each drroom As DataRow In dsrooms.Tables(dsrooms.TBL_ROOM_HOTEL).Rows

            dr1 = dt.NewRow()
            dr1("ROOM") = drroom(dsrooms.FLD_ROOM_CODE)
            dr1("IDROOM") = drroom(dsrooms.FLD_ID_ROOM_HOTEL)
            dr1("DATA") = PortalCulture.GetString("00194") '"1 person"
            dr2 = dt.NewRow()
            dr2("IDROOM") = drroom(dsrooms.FLD_ID_ROOM_HOTEL)
            dr2("DATA") = PortalCulture.GetString("00195") ' "2 person"
            dr3 = dt.NewRow()
            dr3("IDROOM") = drroom(dsrooms.FLD_ID_ROOM_HOTEL)
            dr3("DATA") = PortalCulture.GetString("00196") '"Avail./Total"

            dr4 = dt.NewRow()
            dr4("IDROOM") = drroom(dsrooms.FLD_ID_ROOM_HOTEL)
            dr4("DATA") = PortalCulture.GetString("00199")

            dr5 = dt.NewRow()
            dr5("IDROOM") = drroom(dsrooms.FLD_ID_ROOM_HOTEL)
            dr5("DATA") = PortalCulture.GetString("00200")

            For i As Integer = 0 To DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))

                drRatePlanDate = dtRates.Select( _
                   " idtipohabitacion_hotel=" & drroom(dsrooms.FLD_ID_ROOM_HOTEL) & " and " & _
                   " fecha =#" & CDate(txtDateFrom.Text).AddDays(i).ToString("M/dd/yy") & "#")

                'dvlocks = data.Tables(lockRatePlanData.TABLE_LockRatePlan).DefaultView
                'dvgral = data.Tables(clsCommonAvailibilityGral.TABLE_LockGral).DefaultView
                'dvgral.RowFilter = clsCommonAvailibilityGral.FIELD_StartDate & " ='" & CDate(txtDateFrom.Text).AddDays(i) & "' and " & clsCommonAvailibilityGral.FIELD_statusAvailability & "='C'"
                'dvlocks.RowFilter = clsCommonAvailibilityGral.FIELD_StartDate & " ='" & CDate(txtDateFrom.Text).AddDays(i) & "' and (" & clsCommonAvailibilityGral.FIELD_statusAvailability & "='C'or " & clsCommonAvailibilityGral.FIELD_statusAvailability & "='N')"
                Dim concatstatus As String = ""
                If drRatePlanDate.Length > 0 Then
                    'If dvlocks.Count > 0 Then
                    '    concatstatus = "*" & dvlocks(0)(clsCommonAvailibilityGral.FIELD_statusAvailability).ToString.ToUpper
                    'End If
                    Select Case drRatePlanDate(0)("avail")
                        Case "O"  ' Open
                            dr1(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                            dr4(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                            dr5(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                            dr2(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                        Case "C", "N", "NR", "NA" ' Close
                            Select Case drRatePlanDate(0)("avail")
                                Case "C" : dr1(CDate(txtDateFrom.Text).AddDays(i)) = PortalCulture.GetString("00197")
                                Case "N" : dr1(CDate(txtDateFrom.Text).AddDays(i)) = "No Arrivos" 'PortalCulture.GetString("00197")
                                Case "NR" : dr1(CDate(txtDateFrom.Text).AddDays(i)) = "Sin Tarifa" 'PortalCulture.GetString("00197")
                                Case "NA" : dr1(CDate(txtDateFrom.Text).AddDays(i)) = "Sin Disponibilidad" 'PortalCulture.GetString("00197")
                            End Select
                            '& dvlocks(0)(clsCommonAvailibilityGral.FIELD_statusAvailability).ToString.ToUpper
                    End Select
                    concatstatus = "*" & drRatePlanDate(0)("Avail")
                    'dr1(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    dr4(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    dr5(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    dr2(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    ''tarifas''
                    dr1(CDate(txtDateFrom.Text).AddDays(i)) = drRatePlanDate(0)("Adult1") 'dvRP(0)("tar")
                    dr2(CDate(txtDateFrom.Text).AddDays(i)) = drRatePlanDate(0)("Adult2")
                    dr4(CDate(txtDateFrom.Text).AddDays(i)) = drRatePlanDate(0)("minStay") 'dvRP(0)(FaresData.MINDIAS_FIELD)
                    dr5(CDate(txtDateFrom.Text).AddDays(i)) = drRatePlanDate(0)("maxStay") 'FaresData.MAXDIAS_FIELD)
                    'dr3(CDate(txtDateFrom.Text).AddDays(i)) = "-/-" & concatstatus
                    dr3(CDate(txtDateFrom.Text).AddDays(i)) = drRatePlanDate(0)("Res") & "/" & drRatePlanDate(0)("Rooms") & concatstatus  'dvRes(0)("Inventario") - dvRes(0)("reservaciones") & "/" & dvRes(0)("Inventario") & concatstatus

                    'If dvgral.Count > 0 Then
                    '    'dr1(CDate(txtDateFrom.Text).AddDays(i)) = PortalCulture.GetString("00197")
                    'Else
                    '    dr1(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    '    dr4(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    '    dr5(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    '    dr2(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    '    If DS.Tables.Count > 0 Then
                    '        dvRP = DS.Tables(0).DefaultView
                    '        dvRP.RowFilter = dsrooms.FLD_ID_ROOM_HOTEL & " = " & drroom(dsrooms.FLD_ID_ROOM_HOTEL) & " and " & FaresData.STARTDATE_FIELD & " <='" & CDate(txtDateFrom.Text).AddDays(i) & "' and " & FaresData.ENDDATE_FIELD & ">='" & CDate(txtDateFrom.Text).AddDays(i) & "' and " & " adultos =" & 1
                    '        If dvRP.Count > 0 Then
                    '            dr1(CDate(txtDateFrom.Text).AddDays(i)) = dvRP(0)("tar")
                    '            If Not (dvRP(0)(FaresData.MINDIAS_FIELD)) Is System.DBNull.Value Then
                    '                dr4(CDate(txtDateFrom.Text).AddDays(i)) = dvRP(0)(FaresData.MINDIAS_FIELD)
                    '            End If
                    '            If Not (dvRP(0)(FaresData.MAXDIAS_FIELD)) Is System.DBNull.Value Then
                    '                dr5(CDate(txtDateFrom.Text).AddDays(i)) = dvRP(0)(FaresData.MAXDIAS_FIELD)
                    '            End If
                    '        Else
                    '            dr1(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    '        End If
                    '        dvRP = DS.Tables(0).DefaultView
                    '        dvRP.RowFilter = dsrooms.FLD_ID_ROOM_HOTEL & " = " & drroom(dsrooms.FLD_ID_ROOM_HOTEL) & " and " & FaresData.STARTDATE_FIELD & "<='" & CDate(txtDateFrom.Text).AddDays(i) & "' and " & FaresData.ENDDATE_FIELD & ">='" & CDate(txtDateFrom.Text).AddDays(i) & "' and " & " adultos =" & 2
                    '        If dvRP.Count > 0 Then
                    '            dr2(CDate(txtDateFrom.Text).AddDays(i)) = dvRP(0)("tar")
                    '        End If
                    '    End If
                    '    dr3(CDate(txtDateFrom.Text).AddDays(i)) = "-/-" & concatstatus
                    '    dvRes = data.Tables("Inventario").DefaultView
                    '    dvRes.RowFilter = "fecha ='" & CDate(txtDateFrom.Text).AddDays(i) & "'"
                    '    If dvRes.Count > 0 Then
                    '        'aki es donde tengo ke evaluar los valores
                    '        'columns = SoldOut, AvailLockRPRoom,Inventario
                    '        Dim Reservaciones As Integer = 0
                    '        For Each item As DataRowView In dvRes
                    '            Reservaciones += item("reservaciones")
                    '        Next
                    '        dvRes.RowFilter = dsrooms.FLD_ID_ROOM_HOTEL & " = " & drroom(dsrooms.FLD_ID_ROOM_HOTEL) & " and fecha ='" & CDate(txtDateFrom.Text).AddDays(i) & "'"
                    '        'si la suma de todas las reservaciones en ese rateplan ya son igual al soldout
                    '        'ya no hay disponibilidad
                    '        If dvRes.Count > 0 Then
                    '            If dvRes(0)("SoldOut") Is System.DBNull.Value Then dvRes(0)("SoldOut") = dvRes(0)("Inventario") '0
                    '            If Reservaciones < dvRes(0)("SoldOut") Then
                    '                '  dvRes.RowFilter = dsrooms.FLD_ID_ROOM_HOTEL & " = " & drroom(dsrooms.FLD_ID_ROOM_HOTEL) & " and fecha ='" & CDate(txtDateFrom.Text).AddDays(i) & "'"
                    '                If dvRes(0)("AvailLockRPRoom") Is System.DBNull.Value Then
                    '                    If dvRes(0)("Inventario") < Int(dvRes(0)("SoldOut")) Then
                    '                        dr3(CDate(txtDateFrom.Text).AddDays(i)) = dvRes(0)("Inventario") - dvRes(0)("reservaciones") & "/" & dvRes(0)("Inventario") & concatstatus
                    '                    Else
                    '                        dr3(CDate(txtDateFrom.Text).AddDays(i)) = Int(dvRes(0)("SoldOut")) - dvRes(0)("reservaciones") & "/" & Int(dvRes(0)("SoldOut")) & concatstatus
                    '                    End If
                    '                Else
                    '                    dr3(CDate(txtDateFrom.Text).AddDays(i)) = dvRes(0)("AvailLockRPRoom") - dvRes(0)("reservaciones") & "/" & dvRes(0)("AvailLockRPRoom") & concatstatus
                    '                End If
                    '            Else
                    '                dr3(CDate(txtDateFrom.Text).AddDays(i)) = 0 & "/" & dvRes(0)("reservaciones") & concatstatus
                    '            End If
                    '        End If
                    '    End If

                    'End If
                Else
                    dr1(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    dr4(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    dr5(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    dr2(CDate(txtDateFrom.Text).AddDays(i)) = "NA"
                    ''tarifas''                    
                    dr3(CDate(txtDateFrom.Text).AddDays(i)) = "-/-*NR" '& concatstatus
                    'dvRes(0)("Inventario") - dvRes(0)("reservaciones") & "/" & dvRes(0)("Inventario") & concatstatus
                End If
            Next
            dt.Rows.Add(dr1)
            dt.Rows.Add(dr2)
            dt.Rows.Add(dr4)
            dt.Rows.Add(dr5)
            dt.Rows.Add(dr3)
        Next

        Dim dv As New DataView(dt)
        Return dv

    End Function
    Function CreateDataSourceVertical() As ICollection
        Dim DS As DataSet, RP As RatePlanData, strError As String
        Dim dt As New DataTable, dr As DataRow, dr1 As DataRow, ratesplans As String = ""
        Dim dsrooms As RoomsHotelData, data As DataSet
        Dim dtRates As DataTable
        Dim dtime As DateTime

        With New RoomFacade
            dsrooms = .getRooms(Me.cInfoActual.Hotel)
        End With

        totalrooms = dsrooms.Tables(dsrooms.TBL_ROOM_HOTEL).Rows.Count

        'With New RoomsInventoryFacade
        '    data = .GetLocksAndAvailByRP(Me.cInfoActual.Hotel, CDate(txtDateFrom.Text), CDate(txtDateTo.Text), Me.ddlrateplans.SelectedValue)
        'End With
        'With New FaresSystem
        '    DS = .GetFaresListbyDate(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), Me.cInfoActual.Hotel, Me.ddlrateplans.SelectedValue)
        'End With
        '////////////////////// carga datos disponibilidad ///////

        '// Agrega un día, para que incluya el dia de la fecha final.
        dtime = CDate(txtDateTo.Text).AddDays(1)
        With New clsGetAvail
            dtRates = .GetAvail(CDate(txtDateFrom.Text), dtime, MyBase.cInfoActual.Hotel, ddlrateplans.SelectedItem.Text)
        End With

        dt.Columns.Add(New DataColumn(PortalCulture.GetString("M000120"), GetType(String)))
        For i As Integer = 0 To dsrooms.Tables(dsrooms.TBL_ROOM_HOTEL).Rows.Count - 1
            With dsrooms.Tables(dsrooms.TBL_ROOM_HOTEL).Rows(i)
                dt.Columns.Add(New DataColumn("R" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString & "_" & .Item(dsrooms.FLD_ROOM_CODE).ToString, GetType(String)))
                dt.Columns.Add(New DataColumn("1P" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString, GetType(String)))
                dt.Columns.Add(New DataColumn("2P" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString, GetType(String)))
                dt.Columns.Add(New DataColumn("Min" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString, GetType(String)))
                dt.Columns.Add(New DataColumn("Max" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString, GetType(String)))
                dt.Columns.Add(New DataColumn("Avail" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString, GetType(String)))
            End With
        Next
        Dim dvRP, dvRes, dvgral, dvlocks As DataView
        dr1 = dt.NewRow()
        dr1(PortalCulture.GetString("M000120")) = "*"
        dt.Rows.Add(dr1)
        For i As Integer = 0 To dsrooms.Tables(dsrooms.TBL_ROOM_HOTEL).Rows.Count - 1
            With dsrooms.Tables(dsrooms.TBL_ROOM_HOTEL).Rows(i)
                dr1("R" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString & "_" & .Item(dsrooms.FLD_ROOM_CODE).ToString) = .Item(dsrooms.FLD_ROOM_CODE).ToString
                dr1("1P" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = PortalCulture.GetString("00194")
                dr1("2P" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = PortalCulture.GetString("00195")
                dr1("Min" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "Min"
                dr1("Max" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "Max"
                dr1("Avail" & .Item(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = PortalCulture.GetString("00196")
            End With
        Next
        Dim drRatePlanDate() As DataRow
        For i As Integer = 0 To DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))

            dr1 = dt.NewRow()
            'dr1("Fecha") = CDate(txtDateFrom.Text).AddDays(i)
            dr1(PortalCulture.GetString("M000120")) = CDate(txtDateFrom.Text).AddDays(i)
            dt.Rows.Add(dr1)
            For Each drroom As DataRow In dsrooms.Tables(dsrooms.TBL_ROOM_HOTEL).Rows

                drRatePlanDate = dtRates.Select( _
                    " idtipohabitacion_hotel=" & drroom(dsrooms.FLD_ID_ROOM_HOTEL) & " and " & _
                    " fecha =#" & CDate(txtDateFrom.Text).AddDays(i).ToString("M/dd/yy") & "#")
                If drRatePlanDate.Length > 0 Then
                    'dvgral = data.Tables(clsCommonAvailibilityGral.TABLE_LockGral).DefaultView
                    'dvgral.RowFilter = clsCommonAvailibilityGral.FIELD_StartDate & " ='" & CDate(txtDateFrom.Text).AddDays(i) & "' and " & clsCommonAvailibilityGral.FIELD_statusAvailability & "='C'"
                    'dvlocks = data.Tables(lockRatePlanData.TABLE_LockRatePlan).DefaultView
                    'dvlocks.RowFilter = clsCommonAvailibilityGral.FIELD_StartDate & " ='" & CDate(txtDateFrom.Text).AddDays(i) & "' and (" & clsCommonAvailibilityGral.FIELD_statusAvailability & "='C'or " & clsCommonAvailibilityGral.FIELD_statusAvailability & "='N')"
                    Dim concatstatus As String = ""
                    'If dvlocks.Count > 0 Then
                    '    concatstatus = "*" & dvlocks(0)(clsCommonAvailibilityGral.FIELD_statusAvailability).ToString.ToUpper
                    'End If

                    'If dvgral.Count > 0 Then
                    '    dr1("1P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = PortalCulture.GetString("00197")
                    'Else

                    Select Case drRatePlanDate(0)("avail")
                        Case "O"  ' Open
                            dr1("1P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                            dr1("2P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                            dr1("Min" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                            dr1("Max" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                        Case "C", "N", "NR", "NA" ' Close
                            Select Case drRatePlanDate(0)("avail")
                                Case "C" : dr1("1P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = PortalCulture.GetString("00197")
                                Case "N" : dr1("1P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "No Arrivos" 'PortalCulture.GetString("00197")
                                Case "NR" : dr1("1P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "Sin Tarifa" 'PortalCulture.GetString("00197")
                                Case "NA" : dr1("1P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "Sin Disponibilidad" 'PortalCulture.GetString("00197")
                            End Select
                            '& dvlocks(0)(clsCommonAvailibilityGral.FIELD_statusAvailability).ToString.ToUpper
                    End Select
                    concatstatus = "*" & drRatePlanDate(0)("Avail")
                    ''tarifas''

                    dr1("1P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                    dr1("2P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                    dr1("Min" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                    dr1("Max" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"

                    dr1("1P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = drRatePlanDate(0)("Adult1")  'dvRP(0)("tar")
                    dr1("2P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = drRatePlanDate(0)("Adult2")
                    dr1("Min" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = drRatePlanDate(0)("minStay") 'dvRP(0)(FaresData.MINDIAS_FIELD)
                    dr1("Max" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = drRatePlanDate(0)("maxStay") 'FaresData.MAXDIAS_FIELD)
                    'dr3(CDate(txtDateFrom.Text).AddDays(i)) = "-/-" & concatstatus
                    dr1("Avail" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = drRatePlanDate(0)("Res") & "/" & drRatePlanDate(0)("Rooms") & concatstatus   'dvRes(0)("Inventario") - dvRes(0)("reservaciones") & "/" & dvRes(0)("Inventario") & concatstatus

                    'If DS.Tables.Count > 0 Then
                    '    dvRP = DS.Tables(0).DefaultView
                    '    dvRP.RowFilter = dsrooms.FLD_ID_ROOM_HOTEL & " = " & drroom(dsrooms.FLD_ID_ROOM_HOTEL) & " and " & FaresData.STARTDATE_FIELD & " <='" & CDate(txtDateFrom.Text).AddDays(i) & "' and " & FaresData.ENDDATE_FIELD & ">='" & CDate(txtDateFrom.Text).AddDays(i) & "' and " & " adultos =" & 1
                    '    If dvRP.Count > 0 Then
                    '        dr1("1P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = dvRP(0)("tar")
                    '        If Not (dvRP(0)(FaresData.MINDIAS_FIELD)) Is System.DBNull.Value Then
                    '            dr1("Min" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = dvRP(0)(FaresData.MINDIAS_FIELD)
                    '        End If
                    '        If Not (dvRP(0)(FaresData.MAXDIAS_FIELD)) Is System.DBNull.Value Then
                    '            dr1("Max" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = dvRP(0)(FaresData.MAXDIAS_FIELD)
                    '        End If
                    '    Else
                    '        dr1("1P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                    '    End If
                    '    dvRP = DS.Tables(0).DefaultView
                    '    dvRP.RowFilter = dsrooms.FLD_ID_ROOM_HOTEL & " = " & drroom(dsrooms.FLD_ID_ROOM_HOTEL) & " and " & FaresData.STARTDATE_FIELD & "<='" & CDate(txtDateFrom.Text).AddDays(i) & "' and " & FaresData.ENDDATE_FIELD & ">='" & CDate(txtDateFrom.Text).AddDays(i) & "' and " & " adultos =" & 2
                    '    If dvRP.Count > 0 Then
                    '        dr1("2P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = dvRP(0)("tar")
                    '    End If
                    'End If
                    'dr1("Avail" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "-/-" & concatstatus
                    'dvRes = data.Tables("Inventario").DefaultView
                    'dvRes.RowFilter = "fecha ='" & CDate(txtDateFrom.Text).AddDays(i) & "'"
                    'If dvRes.Count > 0 Then
                    '    'aki es donde tengo ke evaluar los valores
                    '    'columns = SoldOut, AvailLockRPRoom,Inventario
                    '    Dim Reservaciones As Integer = 0
                    '    For Each item As DataRowView In dvRes
                    '        Reservaciones += item("reservaciones")
                    '    Next
                    '    dvRes.RowFilter = dsrooms.FLD_ID_ROOM_HOTEL & " = " & drroom(dsrooms.FLD_ID_ROOM_HOTEL) & " and fecha ='" & CDate(txtDateFrom.Text).AddDays(i) & "'"
                    '    'si la suma de todas las reservaciones en ese rateplan ya son igual al soldout
                    '    'ya no hay disponibilidad
                    '    If dvRes.Count > 0 Then
                    '        If dvRes(0)("SoldOut") Is System.DBNull.Value Then dvRes(0)("SoldOut") = dvRes(0)("Inventario") '0
                    '        If Reservaciones < dvRes(0)("SoldOut") Then
                    '            '  dvRes.RowFilter = dsrooms.FLD_ID_ROOM_HOTEL & " = " & drroom(dsrooms.FLD_ID_ROOM_HOTEL) & " and fecha ='" & CDate(txtDateFrom.Text).AddDays(i) & "'"
                    '            If dvRes(0)("AvailLockRPRoom") Is System.DBNull.Value Then
                    '                If dvRes(0)("Inventario") < Int(dvRes(0)("SoldOut")) Then
                    '                    dr1("Avail" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = dvRes(0)("Inventario") - dvRes(0)("reservaciones") & "/" & dvRes(0)("Inventario") & concatstatus
                    '                Else
                    '                    dr1("Avail" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = Int(dvRes(0)("SoldOut")) - dvRes(0)("reservaciones") & "/" & Int(dvRes(0)("SoldOut")) & concatstatus
                    '                End If
                    '            Else
                    '                dr1("Avail" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = dvRes(0)("AvailLockRPRoom") - dvRes(0)("reservaciones") & "/" & dvRes(0)("AvailLockRPRoom") & concatstatus
                    '            End If
                    '        Else
                    '            dr1("Avail" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = 0 & "/" & dvRes(0)("reservaciones") & concatstatus
                    '        End If
                    '    End If
                    'End If

                    'End If
                Else
                    dr1("1P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                    dr1("2P" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                    dr1("Min" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                    dr1("Max" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "NA"
                    dr1("Avail" & drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString) = "-/-*NR"
                End If
            Next
        Next
        Dim dv As New DataView(dt)
        Return dv

    End Function

    Private Sub btnload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnload.Click
        DifDays = DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        If RdVertical.Checked Then
            Me.dgRatesVertical.DataSource = CreateDataSourceVertical()
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
            Me.dgRatesVertical.DataBind()
            dgRatesVertical.Visible = True
            dgRatesHorizontal.Visible = False
        Else
            Me.dgRatesHorizontal.DataSource = CreateDataSourceHorizontal()
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
            Me.dgRatesHorizontal.DataBind()
            dgRatesVertical.Visible = False
            dgRatesHorizontal.Visible = True
        End If
        System.Threading.Thread.CurrentThread.CurrentCulture = ci


    End Sub
    Private Sub dgRatesVertical_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRatesVertical.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If CType(e.Item.Cells(0), TableCell).Text <> "*" AndAlso CType(e.Item.Cells(0), TableCell).Text <> PortalCulture.GetString("M000120") Then
                Dim mes As String, str As String
                mes = MonthName(CInt(CType(e.Item.Cells(0), TableCell).Text.Substring(0, CType(e.Item.Cells(0), TableCell).Text.IndexOf("/"))), True)
                str = CType(e.Item.Cells(0), TableCell).Text.Substring(CType(e.Item.Cells(0), TableCell).Text.IndexOf("/") + 1)
                CType(e.Item.Cells(0), TableCell).Text = mes & "/" & str.Substring(0, str.IndexOf("/")) 'CDate(CType(e.Item.Cells(0), TableCell).Text).ToString("MMM/dd")
            End If

            For i As Integer = 0 To totalrooms - 1
                If CType(e.Item.Cells(0), TableCell).Text <> "*" AndAlso CType(e.Item.Cells(0), TableCell).Text <> PortalCulture.GetString("M000120") Then

                    If IsNumeric(CType(e.Item.Cells(6 * i + dgcolumns.oneperson), TableCell).Text) Then
                        CType(e.Item.Cells(6 * i + dgcolumns.oneperson), TableCell).Text = FCurrency(CType(e.Item.Cells(6 * i + dgcolumns.oneperson), TableCell).Text, 2)
                    End If
                    If IsNumeric(CType(e.Item.Cells(6 * i + dgcolumns.twoperson), TableCell).Text) Then
                        CType(e.Item.Cells(6 * i + dgcolumns.twoperson), TableCell).Text = FCurrency(CType(e.Item.Cells(6 * i + dgcolumns.twoperson), TableCell).Text, 2)
                    End If
                    CType(e.Item.Cells(6 * i + 1), TableCell).CssClass = "dgAlternate"
                    If CType(e.Item.Cells(6 * i + dgcolumns.oneperson), TableCell).text = PortalCulture.GetString("00197") Then
                        CType(e.Item.Cells(6 * i + dgcolumns.oneperson), TableCell).ForeColor = System.Drawing.Color.Red
                    End If

                    'If CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).Text.Substring(0, 1) = "0" AndAlso CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).Text.Substring(2, 1) <> "0" Then
                    '    CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).ForeColor = System.Drawing.Color.LightSalmon
                    'Else
                    '    CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).ForeColor = System.Drawing.Color.MediumSeaGreen
                    'End If
                    Dim pos As Integer = -1
                    pos = (e.Item.Cells(6 * i + dgcolumns.Avail).Text.IndexOf("*"))
                    If pos <> -1 Then
                        'If CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).Text.Substring(pos + 1, 1).ToUpper.Trim = "C" Then
                        '    CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).ForeColor = System.Drawing.Color.Red
                        'ElseIf CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).Text.Substring(pos + 1, 1).ToUpper.Trim = "N" Then
                        '    CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).ForeColor = System.Drawing.Color.LightSteelBlue
                        'End If
                        Select Case CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).Text.Substring(pos + 1).ToUpper.Trim
                            Case "O"
                                CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).ForeColor = System.Drawing.Color.Green
                            Case "C"
                                CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).ForeColor = System.Drawing.Color.Red
                            Case "N"
                                CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).ForeColor = System.Drawing.Color.LightSteelBlue
                            Case "NR"
                                CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).ForeColor = System.Drawing.Color.DarkGoldenrod
                            Case "NA"
                                CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).ForeColor = System.Drawing.Color.LightSalmon
                        End Select
                        CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).Text = CType(e.Item.Cells(6 * i + dgcolumns.Avail), TableCell).Text.Substring(0, pos)
                    End If
                Else
                    e.Item.CssClass = "dgheader"
                    CType(e.Item.Cells(6 * i + 1), TableCell).text = ""
                    CType(e.Item.Cells(0), TableCell).Text = PortalCulture.GetString("M000120")
                End If
            Next
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.CssClass = "dgItem"
            CType(e.Item.Cells(0), TableCell).Text = ""

            For i As Integer = totalrooms - 1 To 0 Step (-1)
                CType(e.Item.Cells(6 * i + 1), TableCell).Text = CType(e.Item.Cells(6 * i + 1), TableCell).Text.Substring((CType(e.Item.Cells(6 * i + 1), TableCell).Text.IndexOf("_") + 1))
                CType(e.Item.Cells(6 * i + 1), TableCell).ColumnSpan = 6
                CType(e.Item.Cells(6 * i + 1), TableCell).HorizontalAlign = HorizontalAlign.Center
                CType(e.Item.Cells(6 * i + 2), TableCell).Visible = False
                CType(e.Item.Cells(6 * i + 3), TableCell).Visible = False
                CType(e.Item.Cells(6 * i + 4), TableCell).Visible = False
                CType(e.Item.Cells(6 * i + 5), TableCell).Visible = False
                CType(e.Item.Cells(6 * i + 6), TableCell).Visible = False
            Next

        End If


    End Sub


    Private Sub dgRatesHorizontal_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRatesHorizontal.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If CType(e.Item.Cells(2), TableCell).Text <> PortalCulture.GetString("00199") AndAlso CType(e.Item.Cells(2), TableCell).Text <> PortalCulture.GetString("00200") Then
                For i As Integer = 0 To DifDays
                    If IsNumeric(CType(e.Item.Cells(3 + i), TableCell).Text) Then
                        CType(e.Item.Cells(3 + i), TableCell).Text = FCurrency(CType(e.Item.Cells(3 + i), TableCell).Text, 2)
                    End If
                    'If CType(e.Item.Cells(3 + i), TableCell).Text = PortalCulture.GetString("00197") Then
                    '    CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.Red
                    'End If
                    'If CType(e.Item.Cells(2), TableCell).Text = PortalCulture.GetString("00196") Then
                    '    If CType(e.Item.Cells(3 + i), TableCell).Text.Substring(0, 1) = "0" AndAlso CType(e.Item.Cells(3 + i), TableCell).Text.Substring(2, 1) <> "0" Then
                    '        CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.LightSalmon
                    '    Else
                    '        CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.MediumSeaGreen
                    '    End If
                    'End If
                    Dim pos As Integer = -1
                    pos = CType(e.Item.Cells(3 + i), TableCell).Text.IndexOf("*")
                    If pos <> -1 Then
                        Select Case CType(e.Item.Cells(3 + i), TableCell).Text.Substring(pos + 1).ToUpper.Trim
                            Case "O"
                                CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.Green
                            Case "C"
                                CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.Red
                            Case "N"
                                CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.LightSteelBlue
                            Case "NR"
                                CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.DarkGoldenrod
                            Case "NA"
                                CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.LightSalmon
                        End Select
                        'If CType(e.Item.Cells(3 + i), TableCell).Text.Substring(pos + 1, 1).ToUpper.Trim = "C" Then
                        '    CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.Red
                        'ElseIf CType(e.Item.Cells(3 + i), TableCell).Text.Substring(pos + 1, 1).ToUpper.Trim = "N" Then
                        '    CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.LightSteelBlue
                        'End If
                        CType(e.Item.Cells(3 + i), TableCell).Text = CType(e.Item.Cells(3 + i), TableCell).Text.Substring(0, pos)
                    End If
                Next
            End If
        End If
        If e.Item.ItemType = ListItemType.Header Then
            CType(e.Item.Cells(0), TableCell).Text = PortalCulture.GetString("00198")
            CType(e.Item.Cells(2), TableCell).Text = ""
            For i As Integer = 0 To DifDays
                With CType(e.Item.Cells(3 + i), TableCell)
                    Dim mes As String, str As String
                    mes = MonthName(CInt(.Text.Substring(0, .Text.IndexOf("/"))), True)
                    str = .Text.Substring(.Text.IndexOf("/") + 1)
                    .Text = mes & "/" & str.Substring(0, str.IndexOf("/")) 'CDate(CType(e.Item.Cells(0), TableCell).Text).ToString("MMM/dd")


                End With

            Next
        End If
        CType(e.Item.Cells(1), TableCell).Visible = False

    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTitle.Text = PortalCulture.GetString("00202")
        lblstart.Text = PortalCulture.GetString("00108", True)
        lblEnd.Text = PortalCulture.GetString("00109", True)
        lblrateplan.Text = PortalCulture.GetString("00016", True)
        btnload.Text = PortalCulture.GetString("00149", True)
        RdVertical.Text = PortalCulture.GetString("00394", True)
        RdHorizontal.Text = PortalCulture.GetString("00395", True)
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.Tarifas, Me.btnload, "R")
        lblHelp.Text = PortalCulture.GetString("00424")
        Me.ddlrateplans.Attributes.Add("onChange", "javascript:showRatePlan2('" & Me.ddlrateplans.ClientID & "','" & Me.SourceRateName & "','" & lblDescRatePlan.ClientID & "')")

        lblAbierto.Text = PortalCulture.GetString("00150")
        lblCerrado.Text = PortalCulture.GetString("00151")
        lblnoarrivos.Text = PortalCulture.GetString("00152")

        lblNoRates.Text = PortalCulture.GetString("00531")
        lblNodisponible.Text = PortalCulture.GetString("00532")


    End Sub
End Class
