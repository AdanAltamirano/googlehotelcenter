Imports System.Data.SqlClient

Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade

Partial Public Class CtrRatePlanDineroMail
    Inherits System.Web.UI.UserControl

    Public Property idRatePlan() As String
        Get
            Return ViewState("idRatePlan")
        End Get
        Set(ByVal value As String)
            ViewState("idRatePlan") = value
        End Set
    End Property

    Public Property idPaymentModel() As Integer
        Get
            Return ViewState("idPaymentModel")
        End Get
        Set(ByVal value As Integer)
            ViewState("idPaymentModel") = value
        End Set
    End Property

    Private Function CargaMeses() As Boolean
        chkListAmex.Items.Clear()
        chkListTC.Items.Clear()
        For i As Integer = 0 To 3
            chkListTC.Items.Add("")
            chkListAmex.Items.Add("")
        Next
    End Function

    Public Sub ClearCtlrs()
        idPaymentModel = 0
        txtCta.Text = ""
        idRatePlan = ""
        For j As Integer = 0 To chkListTC.Items.Count - 1
            chkListTC.Items(j).Selected = False
        Next

        For j As Integer = 0 To chkListAmex.Items.Count - 1
            chkListAmex.Items(j).Selected = False
        Next
        chkTC.Checked = False
        chkAmex.Checked = False
        chkOxxo.Checked = False
        chkEleven.Checked = False
        chkTL.Checked = False
        chkEfectivo.Checked = False
        chkDineroMail.Checked = False
    End Sub

    Function LoadResource() As Boolean
        Me.lblCuenta.Text = PortalCulture.GetString("M0BT0000192", True)

        Me.lblNota.Text = PortalCulture.GetString("01253")
        Me.chkTC.Text = PortalCulture.GetString("01254")
        Me.chkAmex.Text = PortalCulture.GetString("01255")
        Me.chkOxxo.Text = PortalCulture.GetString("01256")
        Me.chkEleven.Text = PortalCulture.GetString("01257")
        Me.chkTL.Text = PortalCulture.GetString("01258")
        Me.chkEfectivo.Text = PortalCulture.GetString("01259")
        Me.chkDineroMail.Text = PortalCulture.GetString("01260")

        rfvCuenta.Text = PortalCulture.GetString("00785")

    End Function

    Function MetodoDM() As String
        Dim sPagos() As String = {"0", "0", "13,", "14,", "18,", "2,", "7,"}
        Dim sTC() As String = {"4,", "5,", "6,", "17,"}
        Dim sAMEX() As String = {"19,", "20,", "21,", "22,"}
        Dim sSrc As String

        sSrc = ""
        If chkTC.Checked Then
            For i As Integer = 0 To chkListTC.Items.Count - 1
                sSrc &= If(chkListTC.Items(i).Selected, sTC(i), "")
            Next
        End If
        If chkAmex.Checked Then
            For i As Integer = 0 To chkListAmex.Items.Count - 1
                sSrc &= If(chkListAmex.Items(i).Selected, sAMEX(i), "")
            Next
        End If
        sSrc &= If(chkOxxo.Checked, sPagos(2), "")
        sSrc &= If(chkEleven.Checked, sPagos(3), "")
        sSrc &= If(chkTL.Checked, sPagos(4), "")
        sSrc &= If(chkEfectivo.Checked, sPagos(5), "")
        sSrc &= If(chkDineroMail.Checked, sPagos(6), "")
        If sSrc.Length > 1 Then sSrc = If(sSrc.Substring(sSrc.Length - 1) = ",", sSrc.Substring(0, sSrc.Length - 1), sSrc)

        Return sSrc

    End Function

    Function LoadMD(ByVal src As String) As Boolean
        Dim sPagos() As String = {"0", "0", "13", "14", "18", "2", "7"}
        Dim sTC() As String = {"4", "5", "6", "17"}
        Dim sAMEX() As String = {"19", "20", "21", "22"}
        Dim slistMD() As String
        Dim i As Integer
        Dim isChkTC As Boolean
        Dim isChkAMEX As Boolean

        slistMD = src.Split(",")
        If chkListTC.Items.Count = 0 Then
            CargaMeses()
        End If

        For j As Integer = 0 To chkListTC.Items.Count - 1
            chkListTC.Items(j).Selected = False
        Next

        For j As Integer = 0 To chkListAmex.Items.Count - 1
            chkListAmex.Items(j).Selected = False
        Next

        isChkTC = False
        isChkAMEX = False
        chkOxxo.Checked = False
        chkEleven.Checked = False
        chkTL.Checked = False
        chkEfectivo.Checked = False
        chkDineroMail.Checked = False

        For Each item As String In slistMD
            i = Array.IndexOf(sTC, item)
            If i <> -1 Then
                chkListTC.Items(i).Selected = True
                isChkTC = True
            End If
            i = Array.IndexOf(sAMEX, item)
            If i <> -1 Then
                chkListAmex.Items(i).Selected = True
                isChkAMEX = True
            End If
            i = Array.IndexOf(sPagos, item)
            If i <> -1 Then
                Select Case i
                    Case 2
                        chkOxxo.Checked = True
                    Case 3
                        chkEleven.Checked = True
                    Case 4
                        chkTL.Checked = True
                    Case 5
                        chkEfectivo.Checked = True
                    Case 6
                        chkDineroMail.Checked = True
                End Select

            End If
        Next
        chkTC.Checked = isChkTC
        chkAmex.Checked = isChkAMEX

    End Function

    Function getDataXMLRate(ByVal idHotel As Integer) As String
        Dim ds As New RatesPlanPaymentMode
        Dim sError As String = ""
        With New LinkRatePlanFacade
            ds = (New ClsFacadeRatesPlanPaymentModel).GetRatesPlanPaymentModel(idRatePlan, idHotel, sError)
        End With
        Return Util.Utility.GetXml(RatesPlanPaymentMode.NombreTabla, "UpdatePlanFaresNR", ds)
    End Function

    Function getDataXMLHotel(ByVal idHotel As Integer) As String
        Dim ds As New HotelPaymentMode
        Dim sError As String = ""
        With New LinkRatePlanFacade
            ds = (New ClsFacadeHotelPaymentModel).GetHotelPaymentMode(idHotel, sError)
        End With
        Return ds.GetXml
    End Function


    Public Function SaveRatePlanDineroMail() As Boolean
        Dim ds As New RatesPlanPaymentMode
        Dim dr As DataRow
        Dim sError As String = ""
        Dim sData As String = ""
        Dim sDataPrev As String = ""
        Dim Hotel As String
        Dim hr As Boolean
        Dim idhotel As Integer        

        idhotel = CType(Me.Page, PaginaBase).cInfoActual.Hotel
        Hotel = CType(Me.Page, PaginaBase).cInfoActual.HotelName
        If idPaymentModel <> 0 Then sDataPrev = getDataXMLRate(idhotel)

        dr = ds.Tables(RatesPlanPaymentMode.NombreTabla).NewRow
        dr(RatesPlanPaymentMode.FLD_IdPaymentModel) = 0
        dr(RatesPlanPaymentMode.FLD_IdRatePlan) = idRatePlan
        dr(RatesPlanPaymentMode.FLD_Account) = txtCta.Text
        dr(RatesPlanPaymentMode.FLD_IdHotel) = idhotel
        dr(RatesPlanPaymentMode.FLD_PaymentModelSource) = ePaymentTypes.DineroMail
        dr(RatesPlanPaymentMode.FLD_MethodAvailable) = MetodoDM()
        ds.Tables(RatesPlanPaymentMode.NombreTabla).Rows.Add(dr)

        If Not IsDBNull(dr(HotelPaymentMode.FLD_MethodAvailable)) AndAlso Not String.IsNullOrEmpty(dr(HotelPaymentMode.FLD_MethodAvailable)) Then

            If idPaymentModel = 0 Then                
                hr = (New ClsFacadeRatesPlanPaymentModel).InsertRatesPlanPaymentMode(ds, sError)
                If hr Then
                    sData = Util.Utility.GetXml(RatesPlanPaymentMode.NombreTabla, "UpdatePlanFaresNR", ds)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/RatesPlansDineroMail.aspx", PaginaBase.acciones.Crear, String.Format("Se creo dinero Mail para rateplan {0}, hotel {1}", idRatePlan, Hotel), "", sDataPrev, sData)
                End If
            Else
                ds.AcceptChanges()
                dr(RatesPlanPaymentMode.FLD_IdPaymentModel) = idPaymentModel
                hr = (New ClsFacadeRatesPlanPaymentModel).UpdateRatesPlanPaymentMode(ds, sError)
                If hr Then
                    sData = Util.Utility.GetXml(RatesPlanPaymentMode.NombreTabla, "UpdatePlanFaresNR", ds)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/RatesPlansDineroMail.aspx", PaginaBase.acciones.Modificar, String.Format("Se creo dinero Mail para rateplan {0}, hotel {1}", idRatePlan, Hotel), "", sDataPrev, sData)
                End If

            End If
            If hr Then
                ClearCtlrs()
            End If
        End If

        Return hr
    End Function

    Public Function SaveHotelDineroMail(ByVal idHotel As Integer) As Boolean
        Dim ds As New HotelPaymentMode
        Dim dr As DataRow
        Dim sError As String = ""
        Dim sData As String = ""
        Dim sDataPrev As String = ""
        Dim hotel As String
        Dim hr As Boolean

        hotel = CType(Me.Page, PaginaBase).cInfoActual.HotelName
        If idPaymentModel <> 0 Then sDataPrev = getDataXMLHotel(idHotel)

        dr = ds.Tables(HotelPaymentMode.NombreTabla).NewRow
        dr(RatesPlanPaymentMode.FLD_IdPaymentModel) = 0
        dr(HotelPaymentMode.FLD_Account) = txtCta.Text
        dr(HotelPaymentMode.FLD_IdHotel) = idHotel
        dr(HotelPaymentMode.FLD_PaymentModelSource) = ePaymentTypes.DineroMail
        dr(HotelPaymentMode.FLD_MethodAvailable) = MetodoDM()
        ds.Tables(HotelPaymentMode.NombreTabla).Rows.Add(dr)

        If Not IsDBNull(dr(HotelPaymentMode.FLD_MethodAvailable)) AndAlso Not String.IsNullOrEmpty(dr(HotelPaymentMode.FLD_MethodAvailable)) Then
            If idPaymentModel = 0 Then
                hr = (New ClsFacadeHotelPaymentModel).InsertHotelPaymentMode(ds, sError)
                If hr Then
                    CType(Me.Page, PaginaBase).guardalog("/HotelAdministrator/PagesGeneral/Hotel.aspx", PaginaBase.acciones.Crear, "Se creó dinero Mail para el hotel " & hotel, "", sDataPrev, sData)
                End If
            Else
                ds.AcceptChanges()
                dr(RatesPlanPaymentMode.FLD_IdPaymentModel) = idPaymentModel
                hr = (New ClsFacadeHotelPaymentModel).UpdateHotelPaymentMode(ds, sError)
                If hr Then
                    CType(Me.Page, PaginaBase).guardalog("/HotelAdministrator/PagesGeneral/Hotel.aspx", PaginaBase.acciones.Modificar, "Se modifico dinero Mail para el hotel " & hotel, "", sDataPrev, sData)
                End If
            End If
        End If
        If hr Then
            ClearCtlrs()
        End If
        Return hr
    End Function

    Public Function DeleteHotelPaymentMode() As Boolean
        Dim sError As String = ""
        Dim hr As Boolean

        hr = (New ClsFacadeHotelPaymentModel).DeleteHotelPaymentMode(idPaymentModel, sError)
        Return hr
    End Function

    Public Function DeleteRatesPlanPaymentMode() As Boolean
        Dim sError As String = ""
        Dim hr As Boolean
        Dim idhotel As Integer

        idhotel = CType(Me.Page, PaginaBase).cInfoActual.Hotel
        hr = (New ClsFacadeRatesPlanPaymentModel).DeleteRatesPlanPaymentMode(idPaymentModel, sError)
        If hr Then
          ClearCtlrs()
        End If
        Return hr
    End Function

    Public Function LoadDineroMailRatePlan(ByVal idHotel As Integer, ByVal idRatePlans As String)
        Dim ds As New RatesPlanPaymentMode
        Dim dr As DataRow
        Dim sError As String = ""
        Dim hr As Boolean

        hr = False
        idRatePlan = idRatePlans
        ds = (New ClsFacadeRatesPlanPaymentModel).GetRatesPlanPaymentModel(idRatePlans, idHotel, sError)
        If Not ds Is Nothing AndAlso ds.Tables(RatesPlanPaymentMode.NombreTabla).Rows.Count > 0 Then
            dr = ds.Tables(RatesPlanPaymentMode.NombreTabla).Rows(0)

            idPaymentModel = dr(RatesPlanPaymentMode.FLD_IdPaymentModel)
            LoadMD(dr(RatesPlanPaymentMode.FLD_MethodAvailable))
            txtCta.Text = dr(RatesPlanPaymentMode.FLD_Account)
            divContentDineroMail.Style.Add("display", "")
            hr = True
        End If
        Return hr
    End Function

    Public Function LoadDineroMailHotel(ByVal idHotel As Integer)
        Dim ds As New HotelPaymentMode
        Dim dr As DataRow
        Dim sError As String = ""
        Dim hr As Boolean = False

        ds = (New ClsFacadeHotelPaymentModel).GetHotelPaymentMode(idHotel, sError)
        If Not ds Is Nothing AndAlso ds.Tables(HotelPaymentMode.NombreTabla).Rows.Count > 0 Then
            dr = ds.Tables(HotelPaymentMode.NombreTabla).Rows(0)

            idPaymentModel = dr(HotelPaymentMode.FLD_IdPaymentModel)
            LoadMD(dr(HotelPaymentMode.FLD_MethodAvailable))
            txtCta.Text = dr(HotelPaymentMode.FLD_Account)
            hr = True
        End If
        Return hr
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        chkListTC.Attributes.Add("onclick", String.Format("FireChkList('{0}','{1}');", chkListTC.ClientID, chkTC.ClientID))
        chkListAmex.Attributes.Add("onclick", String.Format("FireChkList('{0}','{1}');", chkListAmex.ClientID, chkAmex.ClientID))

        chkTC.Attributes.Add("onclick", String.Format("FireChkListAll('{0}','{1}');", chkTC.ClientID, chkListTC.ClientID))
        chkAmex.Attributes.Add("onclick", String.Format("FireChkListAll('{0}','{1}');", chkAmex.ClientID, chkListAmex.ClientID))

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Me.IsPostBack Then
            If chkListTC.Items.Count = 0 Then
                CargaMeses()
            End If
        End If
        LoadResource()

    End Sub

End Class