Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports APIServices.Models
Imports Portal.Hotel.Common.Data

Namespace Util

    ''' <summary>
    ''' Helpers para construir el XML "antes" de las tarifas eliminadas, de modo
    ''' que LogDetalle pueda mostrar la tabla de lo borrado mediante Tarifas.xslt.
    ''' Debe llamarse SIEMPRE antes de invocar el borrado fisico/logico de la tarifa.
    ''' </summary>
    Public Module DeleteLogHelper

        ''' <summary>
        ''' Construye un XML &lt;Tarifas&gt;&lt;UpdateRate&gt;...&lt;/UpdateRate&gt;&lt;/Tarifas&gt;
        ''' (o UpdateRateNR si la tarifa tiene PrecioNR &gt; 0) para una sola tarifa por id.
        ''' Devuelve String.Empty si la tarifa no existe o ocurre algun error.
        ''' </summary>
        Public Function BuildDeletedRateXml(ByVal rateId As Integer) As String
            Try
                Dim rate As Tarifas = Nothing
                Using db As New OzHotelesEntities()
                    rate = db.Tarifas.AsNoTracking().FirstOrDefault(Function(t) t.idTarifa = rateId)
                End Using
                If rate Is Nothing Then Return String.Empty

                Return BuildXmlForRates(New List(Of Tarifas) From {rate})
            Catch
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Construye un XML &lt;Tarifas&gt;...&lt;/Tarifas&gt; con varias tarifas (caso eliminacion masiva).
        ''' </summary>
        Public Function BuildDeletedRatesXmlByIds(ByVal rateIds As IEnumerable(Of Integer)) As String
            Try
                If rateIds Is Nothing Then Return String.Empty
                Dim ids As Integer() = rateIds.ToArray()
                If ids.Length = 0 Then Return String.Empty

                Dim rates As List(Of Tarifas) = Nothing
                Using db As New OzHotelesEntities()
                    rates = db.Tarifas.AsNoTracking().Where(Function(t) ids.Contains(t.idTarifa)).ToList()
                End Using
                If rates Is Nothing OrElse rates.Count = 0 Then Return String.Empty

                Return BuildXmlForRates(rates)
            Catch
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Construye un XML &lt;Tarifas&gt;...&lt;/Tarifas&gt; con todas las tarifas asociadas a un rate plan
        ''' (incluyendo combinaciones promo+rateplan que terminen o empiecen con el codigo).
        ''' </summary>
        Public Function BuildDeletedRatesXmlByRatePlan(ByVal planCode As String) As String
            Try
                If String.IsNullOrEmpty(planCode) Then Return String.Empty

                Dim rates As List(Of Tarifas) = Nothing
                Using db As New OzHotelesEntities()
                    rates = db.Tarifas.AsNoTracking().Where(
                        Function(t) t.idrateplan = planCode OrElse
                                    t.idrateplan.EndsWith(planCode) OrElse
                                    t.idrateplan.StartsWith(planCode)).ToList()
                End Using
                If rates Is Nothing OrElse rates.Count = 0 Then Return String.Empty

                Return BuildXmlForRates(rates)
            Catch
                Return String.Empty
            End Try
        End Function

        Private Function BuildXmlForRates(ByVal rates As List(Of Tarifas)) As String
            Dim datFare As New FaresData()

            Dim hasNR As Boolean = False

            With datFare.Tables(FaresData.FARES_TABLE)
                For Each rate As Tarifas In rates
                    Dim row As DataRow = .NewRow()
                    row(FaresData.PKIDFARES_FIELD) = rate.idTarifa
                    row(FaresData.HOTELROOMTYPEID_FIELD) = rate.idTipoHabitacion_Hotel
                    row(FaresData.STARTDATE_FIELD) = rate.FechaInicia
                    row(FaresData.ENDDATE_FIELD) = rate.FechaFinaliza
                    row(FaresData.PRICE_FIELD) = rate.Precio
                    row(FaresData.NINIORATE) = If(rate.NiniosRate.HasValue, rate.NiniosRate.Value, 0D)
                    row(FaresData.RATEENPRICE_FIELD) = If(rate.PrecioAdolescente.HasValue, rate.PrecioAdolescente.Value, 0D)
                    row(FaresData.EXTRAADULTPRICE_FIELD) = rate.PrecioExtraAdulto
                    row(FaresData.EXTRACHILDPRICE_FIELD) = rate.PrecioExtraNinio
                    row(FaresData.EXTRATEENPRICE_FIELD) = If(rate.PrecioAdolescenteExtra.HasValue, rate.PrecioAdolescenteExtra.Value, 0D)
                    Dim precioNR As Decimal = If(rate.PrecioNR.HasValue, rate.PrecioNR.Value, 0D)
                    row(FaresData.PRICENR_FIELD) = precioNR
                    If precioNR > 0D Then hasNR = True
                    row(FaresData.NINIORATENR) = If(rate.NiniosRateNR.HasValue, rate.NiniosRateNR.Value, 0D)
                    row(FaresData.RATEENPRICENR_FIELD) = If(rate.PrecioAdolescenteNR.HasValue, rate.PrecioAdolescenteNR.Value, 0D)
                    row(FaresData.EXTRAADULTPRICENR_FIELD) = If(rate.PrecioExtraAdultoNR.HasValue, rate.PrecioExtraAdultoNR.Value, 0D)
                    row(FaresData.EXTRACHILDPRICENR_FIELD) = If(rate.PrecioExtraNinioNR.HasValue, rate.PrecioExtraNinioNR.Value, 0D)
                    row(FaresData.EXTRATEENPRICENR_FIELD) = If(rate.PrecioAdolescenteExtraNR.HasValue, rate.PrecioAdolescenteExtraNR.Value, 0D)
                    row(FaresData.RATETYPE_FIELD) = If(rate.TipoTarifa, String.Empty)
                    row(FaresData.RATECODE_FIELD) = If(rate.CodigoTarifa, String.Empty)
                    row(FaresData.EXCEPTION_FIELD) = If(rate.Excepciones, String.Empty)
                    row(FaresData.NOARRIVOS_FIELD) = If(rate.NoArrivos, String.Empty)
                    row(FaresData.IDRATEPLAN_FIELD) = If(rate.idrateplan, String.Empty)
                    row(FaresData.RULESDEFAULT) = If(rate.RateRulesDefault.HasValue, rate.RateRulesDefault.Value, True)
                    row(FaresData.IDDICCDESCPROM_FIELD) = If(rate.idDiccPromoDesc.HasValue, rate.idDiccPromoDesc.Value, 0)
                    .Rows.Add(row)
                Next
            End With

            ' Columna auxiliar usada por Tarifas.xslt para mostrar habitacion y rate plan
            datFare.Tables(0).Columns.Add("Descr_rateplan")
            For i As Integer = 0 To rates.Count - 1
                datFare.Tables(0).Rows(i)("Descr_rateplan") =
                    String.Format("Hab {0} · {1}", rates(i).idTipoHabitacion_Hotel, rates(i).idrateplan)
            Next

            Dim innerName As String = If(hasNR, "UpdateRateNR", "UpdateRate")
            Return Util.Utility.GetXml(FaresData.FARES_TABLE, innerName, datFare)
        End Function

    End Module

End Namespace
