Imports Portal.General.Facade
Imports Portal.General.Common
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess

Partial Class Hotel
    Inherits PaginaBase
    Private Enum cancelpolicy
        bydays
        byhour
        specifichour
    End Enum

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cbL As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbMa As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbMi As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbJ As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbV As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbS As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbD As System.Web.UI.WebControls.CheckBox
    Protected WithEvents RfvCancel As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ddlCadena As System.Web.UI.WebControls.DropDownList
    Protected txtCancelPolitiesReview As CtrlIdioma
    Protected txtCancelPolitiesFull As CtrlIdioma
    Protected txtCreditCardPolicies As CtrlIdioma
    Protected txtGuarantyPolicies As CtrlIdioma
    Protected txtExtraCharges As CtrlIdioma

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private strError As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not Me.IsPostBack Then
            lblErrorMail.Visible = False
            ddlCancelationPolicy.Items.Clear()
            ddlCancelationPolicy.Items.Insert(cancelpolicy.bydays, PortalCulture.GetString("00020"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.byhour, PortalCulture.GetString("00021"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.specifichour, PortalCulture.GetString("00381"))
            Carga_Monedas()
            CargaPerfiles()
            CargaCorporativos()
            loadData()
        End If
        ddlCancelationPolicy.Attributes.Add("onchange", "javascript:LoadMsg('" & Me.ddlCancelationPolicy.ClientID _
        & "','" & lblAux.ClientID & "','" & lblEDaysHour.ClientID & "','" & PortalCulture.GetString("00410") _
        & "','" & PortalCulture.GetString("00409") & "','" & PortalCulture.GetString("00411") _
        & "','" & PortalCulture.GetString("00412") & "','" & PortalCulture.GetString("00413") & "')")

        txtCancelPolitiesReview.IsMultiline = False
        txtCancelPolitiesReview.MaxLength = 52
        txtExtraCharges.RequiredText = False
    End Sub

    Private Sub CargaPerfiles()
        Me.ddlPerfil.Items.Insert(Me.PerfilHotel.Basico, PortalCulture.GetString("00496"))
        Me.ddlPerfil.Items.Insert(Me.PerfilHotel.Medio, PortalCulture.GetString("00497"))
        Me.ddlPerfil.Items.Insert(Me.PerfilHotel.Avanzado, PortalCulture.GetString("00498"))
        Me.ddlPerfil.Items.Insert(Me.PerfilHotel.NetRate, PortalCulture.GetString("01162"))
        Me.ddlPerfil.Items.Insert(Me.PerfilHotel.Mixto, PortalCulture.GetString("01409"))


    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPublsh.Click
        If validaMail() And validaHours() Then

            Dim publish As Boolean = (CType(sender, Button).ID = Me.btnPublsh.ID)

            save(publish)
        End If
    End Sub

#Region "Funciones generales"

    Private Sub Carga_Monedas()
        cmbMonedas.DataSource = (New MonedaSistema).GetMonedaListIdName
        cmbMonedas.DataTextField = "Nombre"
        cmbMonedas.DataValueField = "idMoneda"
        cmbMonedas.DataBind()
        Try
            Dim dsCulture As New DataSet
            Dim strXML As String = HttpContext.Current.Request.PhysicalApplicationPath & "/PortalCultures.xml"
            dsCulture.ReadXml(strXML)
            Me.ddlEmailLanguage.DataTextField = "name"
            Me.ddlEmailLanguage.DataValueField = "name"
            Me.ddlEmailLanguage.DataSource = dsCulture
            Me.ddlEmailLanguage.DataBind()
            Me.ddlEmailLanguage.Items.Insert(0, PortalCulture.GetString("M000272"))
            Me.ddlEmailLanguage.Items(0).Value = ""
        Catch ex As Exception
        End Try
    End Sub

    Private Sub loadData()
        Dim dsHotel As HotelDatos
        Dim dsEtiq As MonedaDatos

        With New HotelSistema
            dsHotel = .GetHotelById(MyBase.cInfoActual.Hotel)
        End With
        lblAllowDeposit.Visible = True
        Me.chkAllowDeposit.Visible = True
        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                Me.ddlEmailLanguage.SelectedValue = ""
                If Not .Item(dsHotel.FIELD_IDIOMAEMAIL) Is System.DBNull.Value Then
                    Me.ddlEmailLanguage.SelectedValue = .Item(dsHotel.FIELD_IDIOMAEMAIL)
                End If

                chkEsMoroso.Checked = False
                If Not .Item(dsHotel.FIELD_EsMoroso) Is System.DBNull.Value Then
                    Me.chkEsMoroso.Checked = CType(.Item(dsHotel.FIELD_EsMoroso), Boolean)
                End If

                chkEspagoCero.Checked = False
                If Not .Item(dsHotel.FIELD_EsPagoCero) Is System.DBNull.Value Then
                    Me.chkEspagoCero.Checked = CType(.Item(dsHotel.FIELD_EsPagoCero), Boolean)
                End If
                If Not .Item(dsHotel.FIELD_SaveCurrencyShown) Is System.DBNull.Value Then
                    Me.chkSaveCurrencyShown.Checked = CType(.Item(dsHotel.FIELD_SaveCurrencyShown), Boolean)
                End If

                txtCreditCardPolicies.textodefault = .Item(dsHotel.FIELD_POLITICACREDITCARD).ToString
                If Not .IsNull(dsHotel.FIELD_idDiccPoliticaCreditCard) Then
                    txtCreditCardPolicies.CargaDatos(.Item(dsHotel.FIELD_idDiccPoliticaCreditCard))

                    If txtCreditCardPolicies.GetEN <> Nothing Then
                        txtCreditCardPolicies.SetEN(txtCreditCardPolicies.GetEN.Trim)
                    End If

                    If txtCreditCardPolicies.GetES <> Nothing Then
                        txtCreditCardPolicies.setES(txtCreditCardPolicies.GetES.Trim)
                    End If
                End If

                txtGuarantyPolicies.textodefault = .Item(dsHotel.FIELD_POLITICAGARANTIA).ToString
                If Not .IsNull(dsHotel.FIELD_idDiccPoliticaGarantia) Then
                    txtGuarantyPolicies.CargaDatos(.Item(dsHotel.FIELD_idDiccPoliticaGarantia))

                    If txtGuarantyPolicies.GetEN <> Nothing Then
                        txtGuarantyPolicies.SetEN(txtGuarantyPolicies.GetEN.Trim)
                    End If

                    If txtGuarantyPolicies.GetES <> Nothing Then
                        txtGuarantyPolicies.setES(txtGuarantyPolicies.GetES.Trim)
                    End If

                End If

                Dim cancelpolities As String = .Item(dsHotel.FIELD_POLITICACANCELACION).ToString
                txtCancelPolitiesReview.textodefault = cancelpolities.Trim
                If Not .IsNull(dsHotel.FIELD_idDiccPoliticaCancelacionReview) Then
                    txtCancelPolitiesReview.CargaDatos(.Item(dsHotel.FIELD_idDiccPoliticaCancelacionReview))

                    If txtCancelPolitiesReview.GetEN <> Nothing Then
                        txtCancelPolitiesReview.SetEN(txtCancelPolitiesReview.GetEN.Trim)
                    End If

                    If txtCancelPolitiesReview.GetES <> Nothing Then
                        txtCancelPolitiesReview.setES(txtCancelPolitiesReview.GetES.Trim)
                    End If
                End If

                If Not .IsNull(dsHotel.FIELD_idDiccPoliticaCancelacionFull) Then
                    txtCancelPolitiesFull.CargaDatos(.Item(dsHotel.FIELD_idDiccPoliticaCancelacionFull))

                    If txtCancelPolitiesFull.GetEN <> Nothing Then
                        txtCancelPolitiesFull.SetEN(txtCancelPolitiesFull.GetEN.Trim)
                    End If

                    If txtCancelPolitiesFull.GetES <> Nothing Then
                        txtCancelPolitiesFull.setES(txtCancelPolitiesFull.GetES.Trim)
                    End If
                End If

                If cancelpolities.Length > 52 Then
                    txtCancelPolitiesReview.SetEN((cancelpolities.Substring(0, 52)).Trim)
                    'txtCancelPolitiesFull.SetEN((cancelpolities.Substring(52)).Trim)
                End If

                chkEmprTour.Checked = False
                If Not .IsNull(dsHotel.FIELD_EMPRHOTUR) Then
                    chkEmprTour.Checked = .Item(dsHotel.FIELD_EMPRHOTUR)
                End If
                chkAmhm.Checked = False
                If Not .IsNull(dsHotel.FIELD_AMHMRES) Then
                    chkAmhm.Checked = .Item(dsHotel.FIELD_AMHMRES)
                End If
                Me.chkAllowDeposit.Checked = False
                If Not .IsNull(dsHotel.FIELD_AllowBankDeposit) Then
                    Me.chkAllowDeposit.Checked = .Item(dsHotel.FIELD_AllowBankDeposit)
                End If

                txtExtraCharges.textodefault = .Item(dsHotel.FIELD_EXTRACARGOS).ToString
                If Not .IsNull(dsHotel.FIELD_idDiccExtraCargos) Then
                    txtExtraCharges.CargaDatos(.Item(dsHotel.FIELD_idDiccExtraCargos))

                    If txtExtraCharges.GetEN <> Nothing Then
                        txtExtraCharges.SetEN(txtExtraCharges.GetEN.Trim)
                    End If

                    If txtExtraCharges.GetES <> Nothing Then
                        txtExtraCharges.setES(txtExtraCharges.GetES.Trim)
                    End If
                End If

                Me.txtIdGal.Text = .Item(dsHotel.FIELD_PGALILEO).ToString
                Me.txtIdSabre.Text = .Item(dsHotel.FIELD_PSABRE).ToString
                Me.txtIdWorldSpan.Text = .Item(dsHotel.FIELD_PWORLDSPAN).ToString
                Me.txtIdAmadeus.Text = .Item(dsHotel.FIELD_PAMADEUS).ToString

                Me.cmbCategoria.SelectedIndex = cmbCategoria.Items.IndexOf(cmbCategoria.Items.FindByValue(.Item(dsHotel.FIELD_CATEGORIA)))
                Me.cmbMonedas.SelectedIndex = cmbMonedas.Items.IndexOf(cmbMonedas.Items.FindByValue(.Item(dsHotel.FIELD_IDMONEDA)))
                dsEtiq = (New MonedaSistema).GetMonedaById(cmbMonedas.SelectedValue)

                Me.Label2.Text = dsEtiq.Tables(dsEtiq.MONEDA_TABLE).Rows(0).Item(dsEtiq.FIELD_ABREVIATURA)

                Me.cmbCheckinHora.SelectedIndex = cmbCheckinHora.Items.IndexOf(cmbCheckinHora.Items.FindByValue(CDate(.Item(dsHotel.FIELD_CHECKIN)).Hour))
                Me.cmbCheckinMin.SelectedIndex = cmbCheckinMin.Items.IndexOf(cmbCheckinMin.Items.FindByValue(CDate(.Item(dsHotel.FIELD_CHECKIN)).Minute))
                Me.cmbCheckoutHora.SelectedIndex = cmbCheckoutHora.Items.IndexOf(cmbCheckoutHora.Items.FindByValue(CDate(.Item(dsHotel.FIELD_CHECKOUT)).Hour))
                Me.cmbCheckoutMin.SelectedIndex = cmbCheckoutMin.Items.IndexOf(cmbCheckoutMin.Items.FindByValue(CDate(.Item(dsHotel.FIELD_CHECKOUT)).Minute))

                Me.txtImpuesto.Text = Format(.Item(dsHotel.FIELD_IMPUESTO), "###0.00")
                txtImpuestoSrc.Value = Me.txtImpuesto.Text

                If .IsNull(dsHotel.FIELD_SERVICECHARGE) Then
                    Me.txtServiceCharge.Text = 0
                Else
                    Me.txtServiceCharge.Text = Format(.Item(dsHotel.FIELD_SERVICECHARGE), "###0")
                End If

                If .IsNull(dsHotel.FIELD_COMISIONAGENTES) Then
                    Me.txtCommision.Text = 0
                Else
                    Me.txtCommision.Text = Format(.Item(dsHotel.FIELD_COMISIONAGENTES), "###0.00")
                End If

                If Not .IsNull(dsHotel.FIELD_isSingleImgInv) Then
                    chkSingleImgInv.Checked = .Item(dsHotel.FIELD_isSingleImgInv)
                End If

                If Not .IsNull(dsHotel.FIELD_IsPMSPushNotifActive) Then
                    chkPushNotif.Checked = .Item(dsHotel.FIELD_IsPMSPushNotifActive)
                End If

                If .IsNull(dsHotel.FIELD_ECOTASA) Then
                    Me.txtEcotasa.Text = 0
                Else
                    Me.txtEcotasa.Text = Format(.Item(dsHotel.FIELD_ECOTASA), "###0.00")
                End If


                Dim sFecha As String
                sFecha = IIf(.IsNull(dsHotel.FIELD_FECHAAPERTURA), "", .Item(dsHotel.FIELD_FECHAAPERTURA))
                If sFecha <> "" Then sFecha = CDate(sFecha).ToString("MM/dd/yyyy")

                Me.txtEmailReservas.Text = IIf(.IsNull(dsHotel.FIELD_EMAIL_RESERVAS), "", .Item(dsHotel.FIELD_EMAIL_RESERVAS))

                chkPlusTax.Checked = IIf(.IsNull(dsHotel.FIELD_PLUSTAX), False, .Item(dsHotel.FIELD_PLUSTAX))
                chkPlusTaxSrc.Checked = IIf(.IsNull(dsHotel.FIELD_PLUSTAX), False, .Item(dsHotel.FIELD_PLUSTAX))
                chkTransUp.Checked = IIf(.IsNull(dsHotel.FIELD_TransByEmail), False, .Item(dsHotel.FIELD_TransByEmail))

                Me.txtMaxDiasRenta.Text = .Item(dsHotel.FIELD_MAXDIASRENTA)
                Me.txtDiasAnticipados.Text = .Item(dsHotel.FIELD_DIASLIBRES)
                'Me.txtMinDiasCancelar.Text = .Item(dsHotel.FIELD_DIASMINCANCELAR)
                Me.txtCancellationPolicy.Style.Add("display", "")
                Me.ddlHour.Style.Add("display", "none")
                Me.lblSep.Style.Add("display", "none")
                Me.ddlMinutes.Style.Add("display", "none")
                If Not .IsNull(dsHotel.FIELD_DIASMINCANCELAR) Then
                    Me.txtCancellationPolicy.Text = CInt(Val(.Item(dsHotel.FIELD_DIASMINCANCELAR)))
                    Me.ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays
                    Me.lblAux.Text = PortalCulture.GetString("00413")
                    If CInt(Val(.Item(dsHotel.FIELD_DIASMINCANCELAR))) = 0 Then
                        Me.chkNonCancelable.Checked = True
                    End If
                    lblEDaysHour.Text = PortalCulture.GetString("00410")
                ElseIf Not .IsNull(dsHotel.FIELD_CancelHours) Then
                    txtCancellationPolicy.Text = CInt(Val(.Item(dsHotel.FIELD_CancelHours).ToString))
                    Me.ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour
                    Me.lblAux.Text = PortalCulture.GetString("00413")
                    lblEDaysHour.Text = PortalCulture.GetString("00409")
                ElseIf Not .IsNull(dsHotel.FIELD_CancelSpecificHour) Then
                    'txtCancellationPolicy.Text = .Item(dsHotel.FIELD_CancelSpecificHour).ToString
                    'cargar en los ddl la hora
                    Me.ddlCancelationPolicy.SelectedIndex = cancelpolicy.specifichour
                    Me.lblAux.Text = PortalCulture.GetString("00412")
                    lblEDaysHour.Text = PortalCulture.GetString("00411")
                    Me.txtCancellationPolicy.Style.Add("display", "none")
                    Me.ddlHour.Style.Add("display", "")
                    Me.ddlMinutes.Style.Add("display", "")
                    Me.lblSep.Style.Add("display", "")
                    Try
                        Dim hora As String = .Item(dsHotel.FIELD_CancelSpecificHour).ToString
                        Me.ddlHour.SelectedValue = hora.Substring(0, 2)
                        If CInt(hora.Substring(2, 2)) < 15 Then
                            Me.ddlMinutes.SelectedValue = "00"
                        ElseIf CInt(hora.Substring(2, 2)) < 30 Then
                            Me.ddlMinutes.SelectedValue = "15"
                        ElseIf CInt(hora.Substring(2, 2)) < 45 Then
                            Me.ddlMinutes.SelectedValue = "30"
                        Else
                            Me.ddlMinutes.SelectedValue = "45"
                        End If
                    Catch ex As Exception
                        Me.ddlHour.SelectedValue = "01"
                        Me.ddlMinutes.SelectedValue = "00"
                    End Try
                End If
                Me.txtEdadMaximaNino.Text = .Item(dsHotel.FIELD_MAXEDADNINO)

                Me.txtPropertyNumber.Text = "" & .Item(dsHotel.FIELD_PROPERTY_NUMBER)
                Me.txtChainCode.Text = "" & .Item(dsHotel.FIELD_CHAIN_CODE)

                Me.rbGetRates_True.Checked = IIf(.IsNull(dsHotel.FIELD_GET_RATES) OrElse .Item(dsHotel.FIELD_GET_RATES) = 0, False, True)
                Me.rbGetRates_False.Checked = Not Me.rbGetRates_True.Checked

                '*****
                sFecha = IIf(.IsNull(dsHotel.FIELD_STARTDATE), "", .Item(dsHotel.FIELD_STARTDATE))
                If sFecha <> "" Then sFecha = CDate(sFecha).ToString("MM/dd/yyyy")

                sFecha = IIf(.IsNull(dsHotel.FIELD_ENDDATE), "", .Item(dsHotel.FIELD_ENDDATE))
                If sFecha <> "" Then sFecha = CDate(sFecha).ToString("MM/dd/yyyy")

                Dim Departure As String
                Dim aux As Char


                Departure = IIf(.IsNull(dsHotel.FIELD_NOARRIVALS), "NNNNNNN", .Item(dsHotel.FIELD_NOARRIVALS))
                aux = Departure.Chars(0)
                If aux = "Y" Then
                    L.Checked = True
                Else
                    L.Checked = False
                End If
                aux = Departure.Chars(1)
                If aux = "Y" Then
                    Ma.Checked = True
                Else
                    Ma.Checked = False
                End If
                aux = Departure.Chars(2)
                If aux = "Y" Then
                    Mi.Checked = True
                Else
                    Mi.Checked = False
                End If
                aux = Departure.Chars(3)
                If aux = "Y" Then
                    J.Checked = True
                Else
                    J.Checked = False
                End If
                aux = Departure.Chars(4)
                If aux = "Y" Then
                    V.Checked = True
                Else
                    V.Checked = False
                End If
                aux = Departure.Chars(5)
                If aux = "Y" Then
                    S.Checked = True
                Else
                    S.Checked = False
                End If
                aux = Departure.Chars(6)
                If aux = "Y" Then
                    D.Checked = True
                Else
                    D.Checked = False
                End If

                Me.txtEstanciaMin.Text = IIf(.IsNull(dsHotel.FIELD_MINLENGTHSTAY), 1, .Item(dsHotel.FIELD_MINLENGTHSTAY))
                If Me.txtEstanciaMin.Text = "0" Then
                    Me.txtEstanciaMin.Text = 1
                End If
                If (.IsNull(dsHotel.FIELD_STATUSAVAILABILITY)) Then
                    Me.ddlStatus.SelectedIndex = ddlStatus.SelectedValue = "O" '.Items.IndexOf(ddlStatus.Items.FindByValue("A"))
                Else
                    Me.ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(.Item(dsHotel.FIELD_STATUSAVAILABILITY)))
                End If
                If Not .IsNull(dsHotel.FIELD_SENDFAXRESERVA) Then
                    Me.chkFaxSend.Checked = .Item(dsHotel.FIELD_SENDFAXRESERVA)
                End If
                If Not .IsNull(dsHotel.FIELD_MAXNUMCUARTOS) Then
                    Me.txtMaxCuartos.Text = .Item(dsHotel.FIELD_MAXNUMCUARTOS)
                End If
                Me.ddlPerfil.SelectedIndex = PerfilHotel.Avanzado
                If Not .IsNull(dsHotel.FIELD_UserPerfil) Then
                    Me.ddlPerfil.SelectedIndex = .Item(dsHotel.FIELD_UserPerfil)
                End If
                Me.chkAvailOnGDS.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnGDS) OrElse .Item(dsHotel.FIELD_AvailOnGDS) = 0, False, True)
                Me.chkAvailOnPortal.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnPortal) OrElse .Item(dsHotel.FIELD_AvailOnPortal) = 0, False, True)
                Me.chkAvailOnOnePage.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnOnePage) OrElse .Item(dsHotel.FIELD_AvailOnOnePage) = 0, False, True)
                Me.chkAvailOnADS.Checked = IIf(.IsNull(dsHotel.FIELD_AvailOnADS) OrElse .Item(dsHotel.FIELD_AvailOnADS) = 0, False, True)

                txtLatitud.Text = "" & .Item(dsHotel.FIELD_latitud)

                If Not .IsNull(dsHotel.FIELD_MinEdadNinio) Then
                    Me.txtEdadMinimaNino.Text = .Item(dsHotel.FIELD_MinEdadNinio)
                Else
                    Me.txtEdadMinimaNino.Text = "0"
                End If

                If Not .IsNull(dsHotel.FIELD_MinNumCuartos) Then
                    Me.txtMinCuartos.Text = .Item(dsHotel.FIELD_MinNumCuartos)
                Else
                    Me.txtMinCuartos.Text = "1"
                End If

                If Not .IsNull(dsHotel.FIELD_MinNumOcupacion) Then
                    Me.txtMinOcupacion.Text = .Item(dsHotel.FIELD_MinNumOcupacion)
                Else
                    Me.txtMinOcupacion.Text = ""
                End If

                If Not .IsNull(dsHotel.FIELD_MaxNumOcupacion) Then
                    Me.txtMaxOcupacion.Text = .Item(dsHotel.FIELD_MaxNumOcupacion)
                Else
                    Me.txtMaxOcupacion.Text = ""
                End If
                txtLongitud.Text = "" & .Item(dsHotel.FIELD_longitud)

                Try
                    If Not .IsNull(HotelDatos.fld_idcorporativo) Then
                        ddlCorporativos.SelectedValue = .Item(HotelDatos.fld_idcorporativo)
                    End If
                Catch ex As Exception

                End Try
                txtEdadMaximaAdo.Text = If(Not .IsNull(dsHotel.FIELD_EdadAdolescente), .Item(dsHotel.FIELD_EdadAdolescente), "")

                If Not .IsNull(dsHotel.FIELD_AVLONCORPMODULE) Then
                    Trace.Write("avlnotnull", .Item(dsHotel.FIELD_AVLONCORPMODULE))
                    chkAvlOnCorpModule.Checked = .Item(dsHotel.FIELD_AVLONCORPMODULE)
                Else
                    Trace.Write("avlnull", "null")
                    chkAvlOnCorpModule.Checked = False
                End If
            End With
        End If
    End Sub

    Private Function ValidaPropertys() As Boolean
        Dim Propertys As HotelDatos

        With New Hoteles
            Propertys = .LoadPropetysId()
        End With


        For i As Integer = 0 To Propertys.Tables(0).Rows.Count - 1
            With Propertys.Tables(0).Rows(i)
                If .Item(Propertys.FIELD_PKID) <> MyBase.cInfoActual.Hotel Then
                    If .Item(Propertys.FIELD_PAMADEUS).ToString <> "" AndAlso Me.txtIdAmadeus.Text = .Item(Propertys.FIELD_PAMADEUS).ToString Then
                        Me.lblErrorPAmadeus.Visible = True
                        Return False
                    Else
                        Me.lblErrorPAmadeus.Visible = False
                    End If
                    If .Item(Propertys.FIELD_PGALILEO).ToString <> "" AndAlso Me.txtIdGal.Text = .Item(Propertys.FIELD_PGALILEO).ToString Then
                        Me.lblErrorPGalileo.Visible = True
                        Return False
                    Else
                        Me.lblErrorPGalileo.Visible = False
                    End If
                    If .Item(Propertys.FIELD_PSABRE).ToString <> "" AndAlso Me.txtIdSabre.Text = .Item(Propertys.FIELD_PSABRE).ToString Then
                        Me.lblErrorPSabre.Visible = True
                        Return False
                    Else
                        Me.lblErrorPSabre.Visible = False

                    End If
                    If .Item(Propertys.FIELD_PWORLDSPAN).ToString <> "" AndAlso Me.txtIdWorldSpan.Text = .Item(Propertys.FIELD_PWORLDSPAN).ToString Then
                        Me.lblErrorPWorld.Visible = True
                        Return False
                    End If
                End If
            End With
        Next
        Return True
    End Function

    Function getDataXML(ByVal id As Integer) As String
        Dim dsHotel As HotelDatos

        With New HotelSistema
            dsHotel = .GetHotelById(id)
        End With
        Return Util.Utility.GetXml(HotelDatos.HOTEL_TABLE, "UpdateHotel", dsHotel)
        'Return dsHotel.GetXml
    End Function

    Sub LoadDsImpuesto(ByVal ds As DataSet)
        Dim dr As DataRow
        Dim impuesto As Double
        Dim impuestoNuevo As Double

        impuesto = 0
        impuestoNuevo = 0
        If Not Me.chkPlusTaxSrc.Checked And Me.chkPlusTax.Checked Then '// Se va incluir impuesto a la tarifa.
            impuesto = 0
            If txtImpuesto.Text <> "" Then
                Double.TryParse(txtImpuesto.Text, impuestoNuevo)
            End If
        ElseIf (Me.chkPlusTaxSrc.Checked And Not Me.chkPlusTax.Checked) Then '// Ya tenia impuestos incluidos y se quitan
            If txtImpuestoSrc.Value <> "" Then
                Double.TryParse(txtImpuestoSrc.Value, impuesto)
            End If
            impuestoNuevo = 0            
        ElseIf (Me.chkPlusTaxSrc.Checked And Me.chkPlusTax.Checked) Then '// Ya tenia impuestos incluidos y se quito
            If (Me.txtImpuesto.Text <> Me.txtImpuestoSrc.Value) Then
                If txtImpuestoSrc.Value <> "" Then
                    Double.TryParse(txtImpuestoSrc.Value, impuesto)
                End If
                If txtImpuesto.Text <> "" Then
                    Double.TryParse(txtImpuesto.Text, impuestoNuevo)
                End If
            End If
        End If

        dr = ds.Tables(0).NewRow()
        dr("idHotel") = MyBase.cInfoActual.Hotel
        dr("impuesto") = impuesto
        dr("impuestoNuevo") = impuestoNuevo
        ds.Tables(0).Rows.Add(dr)
        Trace.Write("salio", "Load Impuestos")
    End Sub

    Private Sub save(ByVal publish As Boolean)

        If Not Page.IsValid Then Return
        If Not ValidaPropertys() Then Return
        Dim dsHotel As HotelDatos
        Dim ds As DataSet
        Dim sDataPrev As String = ""
        Dim sData As String = ""

        ds = BuildDataImpuestos()
        With New HotelSistema
            dsHotel = .GetHotelById(MyBase.cInfoActual.Hotel)

        End With
        'sDataPrev = dsHotel.GetXml
        sDataPrev = Util.Utility.GetXml(HotelDatos.HOTEL_TABLE, "UpdateHotel", dsHotel)

        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                .Item(dsHotel.FIELD_IDIOMAEMAIL) = System.DBNull.Value
                If Me.ddlEmailLanguage.SelectedValue <> "" Then
                    .Item(dsHotel.FIELD_IDIOMAEMAIL) = ddlEmailLanguage.SelectedValue
                End If

                .Item(dsHotel.FIELD_EsMoroso) = Me.chkEsMoroso.Checked
                .Item(dsHotel.FIELD_EsPagoCero) = Me.chkEspagoCero.Checked

                .Item(dsHotel.FIELD_CATEGORIA) = Me.cmbCategoria.SelectedValue

                .Item(dsHotel.FIELD_IDMONEDA) = Me.cmbMonedas.SelectedValue

                Dim strCheckin As Date
                Dim strCheckOut As Date
                strCheckin = CDate(Format(Date.Now, "yyyy/MM/dd ") & cmbCheckinHora.SelectedValue & ":" & cmbCheckinMin.SelectedValue)
                strCheckOut = CDate(Format(Date.Now, "yyyy/MM/dd ") & cmbCheckoutHora.SelectedValue & ":" & cmbCheckoutMin.SelectedValue)

                .Item(dsHotel.FIELD_CHECKIN) = strCheckin
                .Item(dsHotel.FIELD_CHECKOUT) = strCheckOut

                .Item(dsHotel.FIELD_IMPUESTO) = Me.txtImpuesto.Text
                .Item(dsHotel.FIELD_SaveCurrencyShown) = Me.chkSaveCurrencyShown.Checked

                If Me.txtServiceCharge.Text.Trim <> "" Then
                    .Item(dsHotel.FIELD_SERVICECHARGE) = Me.txtServiceCharge.Text.Trim
                Else
                    .Item(dsHotel.FIELD_SERVICECHARGE) = System.DBNull.Value
                End If

                If Me.txtCommision.Text.Trim <> "" Then
                    .Item(dsHotel.FIELD_COMISIONAGENTES) = Me.txtCommision.Text.Trim
                Else
                    .Item(dsHotel.FIELD_COMISIONAGENTES) = System.DBNull.Value
                End If

                .Item(dsHotel.FIELD_TransByEmail) = chkTransUp.Checked

                .Item(dsHotel.FIELD_EMAIL_RESERVAS) = Me.txtEmailReservas.Text.Trim

                .Item(dsHotel.FIELD_DIASLIBRES) = Me.txtDiasAnticipados.Text
                .Item(dsHotel.FIELD_CancelHours) = System.DBNull.Value
                .Item(dsHotel.FIELD_CancelSpecificHour) = System.DBNull.Value
                .Item(dsHotel.FIELD_DIASMINCANCELAR) = System.DBNull.Value
                If Not chkNonCancelable.Checked Then
                    If Me.ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays Then
                        .Item(dsHotel.FIELD_DIASMINCANCELAR) = CInt(Val(Me.txtCancellationPolicy.Text))

                    ElseIf Me.ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour Then
                        .Item(dsHotel.FIELD_CancelHours) = CInt(Val(txtCancellationPolicy.Text))
                    Else
                        .Item(dsHotel.FIELD_CancelSpecificHour) = Me.ddlHour.SelectedValue.ToString & Me.ddlMinutes.SelectedValue.ToString 'Me.txtCancellationPolicy.Text
                    End If
                Else
                    .Item(dsHotel.FIELD_DIASMINCANCELAR) = 0
                    .Item(dsHotel.FIELD_CancelHours) = 0
                End If
                

                .Item(dsHotel.FIELD_PROPERTY_NUMBER) = Me.txtPropertyNumber.Text
                .Item(dsHotel.FIELD_CHAIN_CODE) = (Me.txtChainCode.Text).ToUpper
                If Me.chkPlusTax.Checked Then
                    .Item(dsHotel.FIELD_PLUSTAX) = 1
                Else
                    .Item(dsHotel.FIELD_PLUSTAX) = 0
                End If
                Dim minNoches As Integer
                Integer.TryParse(Me.txtEstanciaMin.Text, minNoches)
                .Item(dsHotel.FIELD_MINLENGTHSTAY) = minNoches
                .Item(dsHotel.FIELD_STATUSAVAILABILITY) = Me.ddlStatus.SelectedValue

                If Me.txtCreditCardPolicies.textodefault.Length > 400 Then
                    .Item(dsHotel.FIELD_POLITICACREDITCARD) = Me.txtCreditCardPolicies.textodefault.Substring(0, 400)
                Else
                    .Item(dsHotel.FIELD_POLITICACREDITCARD) = Me.txtCreditCardPolicies.textodefault
                End If

                If validaIdDicc(.IsNull(dsHotel.FIELD_idDiccPoliticaCreditCard), txtCreditCardPolicies.GetES, txtCreditCardPolicies.GetEN) = True Then
                    .Item(dsHotel.FIELD_idDiccPoliticaCreditCard) = txtCreditCardPolicies.Insert()
                End If

                If Me.txtGuarantyPolicies.textodefault.Length > 400 Then
                    .Item(dsHotel.FIELD_POLITICAGARANTIA) = Me.txtGuarantyPolicies.textodefault.Substring(0, 400)
                Else
                    .Item(dsHotel.FIELD_POLITICAGARANTIA) = Me.txtGuarantyPolicies.textodefault
                End If

                If validaIdDicc(.IsNull(dsHotel.FIELD_idDiccPoliticaGarantia), txtGuarantyPolicies.GetES, txtGuarantyPolicies.GetEN) = True Then
                    .Item(dsHotel.FIELD_idDiccPoliticaGarantia) = txtGuarantyPolicies.Insert()
                End If

                Dim CancelPolities As String
                CancelPolities = txtCancelPolitiesReview.textodefault.PadRight(52)

                If Me.txtCancelPolitiesFull.textodefault.Length > 184 Then
                    .Item(dsHotel.FIELD_POLITICACANCELACION) = CancelPolities & Me.txtCancelPolitiesFull.textodefault.Substring(0, 184)
                Else
                    .Item(dsHotel.FIELD_POLITICACANCELACION) = CancelPolities & Me.txtCancelPolitiesFull.textodefault
                End If

                If validaIdDicc(.IsNull(dsHotel.FIELD_idDiccPoliticaCancelacionReview), txtCancelPolitiesReview.GetES, txtCancelPolitiesReview.GetEN) Then
                    .Item(dsHotel.FIELD_idDiccPoliticaCancelacionReview) = txtCancelPolitiesReview.Insert()
                End If
                If validaIdDicc(.IsNull(dsHotel.FIELD_idDiccPoliticaCancelacionFull), txtCancelPolitiesFull.GetES, txtCancelPolitiesFull.GetEN) Then
                    .Item(dsHotel.FIELD_idDiccPoliticaCancelacionFull) = txtCancelPolitiesFull.Insert()
                End If

                If Me.txtExtraCharges.textodefault.Length > 400 Then
                    .Item(dsHotel.FIELD_EXTRACARGOS) = Me.txtExtraCharges.textodefault.Substring(0, 400)
                Else
                    .Item(dsHotel.FIELD_EXTRACARGOS) = Me.txtExtraCharges.textodefault
                End If

                If validaIdDicc(.IsNull(dsHotel.FIELD_idDiccExtraCargos), txtExtraCharges.GetES, txtExtraCharges.GetEN) Then
                    .Item(dsHotel.FIELD_idDiccExtraCargos) = txtExtraCharges.Insert()
                End If

                .Item(dsHotel.FIELD_EMPRHOTUR) = chkEmprTour.Checked
                .Item(dsHotel.FIELD_AMHMRES) = chkAmhm.Checked

                .Item(dsHotel.FIELD_PGALILEO) = Me.txtIdGal.Text
                .Item(dsHotel.FIELD_PSABRE) = Me.txtIdSabre.Text
                .Item(dsHotel.FIELD_PWORLDSPAN) = Me.txtIdWorldSpan.Text
                .Item(dsHotel.FIELD_PAMADEUS) = Me.txtIdAmadeus.Text
                .Item(dsHotel.FIELD_GET_RATES) = IIf(Me.rbGetRates_True.Checked, True, False)

                Dim Res As String
                Res = saveDeparture()

                .Item("NoArrivals") = Res

                .Item(dsHotel.FIELD_MAXEDADNINO) = Me.txtEdadMaximaNino.Text

                .Item(dsHotel.FIELD_SENDFAXRESERVA) = Me.chkFaxSend.Checked

                .Item(dsHotel.FIELD_MAXNUMCUARTOS) = txtMaxCuartos.Text
                .Item(dsHotel.FIELD_UserPerfil) = Me.ddlPerfil.SelectedIndex

                If IsNumeric(Me.txtMaxDiasRenta.Text) Then .Item(dsHotel.FIELD_MAXDIASRENTA) = CInt(Me.txtMaxDiasRenta.Text)
                .Item(dsHotel.FIELD_AvailOnGDS) = Me.chkAvailOnGDS.Checked
                .Item(dsHotel.FIELD_AvailOnOnePage) = Me.chkAvailOnOnePage.Checked
                .Item(dsHotel.FIELD_AvailOnPortal) = Me.chkAvailOnPortal.Checked
                .Item(dsHotel.FIELD_AvailOnADS) = Me.chkAvailOnADS.Checked
                .Item(dsHotel.FIELD_AVLONCORPMODULE) = Me.chkAvlOnCorpModule.Checked

                .Item(dsHotel.FIELD_latitud) = Val(txtLatitud.Text)
                .Item(dsHotel.FIELD_longitud) = Val(txtLongitud.Text)

                .Item(dsHotel.FIELD_AllowBankDeposit) = Me.chkAllowDeposit.Checked
                .Item(dsHotel.FIELD_isSingleImgInv) = Me.chkSingleImgInv.Checked


                If Not ddlCorporativos.SelectedValue = "" AndAlso ddlCorporativos.SelectedValue <> 0 Then
                    .Item(HotelDatos.fld_idcorporativo) = ddlCorporativos.SelectedValue
                Else
                    .Item(HotelDatos.fld_idcorporativo) = System.DBNull.Value
                End If

                '-Min Ninios
                If txtEdadMinimaNino.Text.Trim <> "" Then
                    .Item(HotelDatos.FIELD_MinEdadNinio) = txtEdadMinimaNino.Text.Trim
                Else
                    .Item(HotelDatos.FIELD_MinEdadNinio) = 0
                End If
                '-Min Cuartos 
                If txtMinCuartos.Text.Trim <> "" Then
                    .Item(HotelDatos.FIELD_MinNumCuartos) = txtMinCuartos.Text.Trim
                Else
                    .Item(HotelDatos.FIELD_MinNumCuartos) = 1
                End If
                '-Ocupacion
                If txtMinOcupacion.Text.Trim <> "" Then
                    .Item(HotelDatos.FIELD_MinNumOcupacion) = txtMinOcupacion.Text.Trim
                Else
                    .Item(HotelDatos.FIELD_MinNumOcupacion) = System.DBNull.Value
                End If
                If txtMaxOcupacion.Text.Trim <> "" Then
                    .Item(HotelDatos.FIELD_MaxNumOcupacion) = txtMaxOcupacion.Text.Trim
                Else
                    .Item(HotelDatos.FIELD_MaxNumOcupacion) = System.DBNull.Value
                End If
                If txtEdadMaximaAdo.Text.Trim <> "" Then
                    .Item(HotelDatos.FIELD_EdadAdolescente) = txtEdadMaximaAdo.Text
                Else
                    .Item(HotelDatos.FIELD_EdadAdolescente) = System.DBNull.Value
                End If

                .Item(HotelDatos.FIELD_IsPMSPushNotifActive) = chkPushNotif.Checked

                If Me.txtEcotasa.Text.Trim <> "" Then
                    .Item(dsHotel.FIELD_ECOTASA) = Me.txtEcotasa.Text.Trim
                Else
                    .Item(dsHotel.FIELD_ECOTASA) = System.DBNull.Value
                End If

            End With

            LoadDsImpuesto(ds)
            Dim hr As Boolean
            With New Hoteles
                If hiddenPostBack.Value.ToLower = "true" Then

                    hr = .ActualizaHotel(dsHotel, ds)
                Else

                    hr = .ActualizaHotel(dsHotel, strError)
                    Trace.Write("actualizaHotel", hr)
                End If
                'If .ActualizaHotel(dsHotel, ds) Then
                If hr Then
                    If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
                        With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                            If Not .IsNull(dsHotel.FIELD_idDiccPoliticaCreditCard) Then
                                txtCreditCardPolicies.Update(dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaCreditCard), validaTexto(False, txtCreditCardPolicies, dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaCreditCard)), validaTexto(True, txtCreditCardPolicies, dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaCreditCard)), publish)
                            End If

                            If Not .IsNull(dsHotel.FIELD_idDiccPoliticaGarantia) Then
                                txtGuarantyPolicies.Update(dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaGarantia), validaTexto(False, txtGuarantyPolicies, dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaGarantia)), validaTexto(True, txtGuarantyPolicies, dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaGarantia)), publish)
                            End If

                            If Not .IsNull(dsHotel.FIELD_idDiccPoliticaCancelacionReview) Then
                                txtCancelPolitiesReview.Update(dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaCancelacionReview), validaTexto(False, txtCancelPolitiesReview, dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaCancelacionReview)), validaTexto(True, txtCancelPolitiesReview, dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaCancelacionReview)), publish)
                            End If

                            If Not .IsNull(dsHotel.FIELD_idDiccPoliticaCancelacionFull) Then
                                txtCancelPolitiesFull.Update(dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaCancelacionFull), validaTexto(False, txtCancelPolitiesFull, dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaCancelacionFull)), validaTexto(True, txtCancelPolitiesFull, dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccPoliticaCancelacionFull)), publish)
                            End If

                            If Not .IsNull(dsHotel.FIELD_idDiccExtraCargos) Then
                                txtExtraCharges.Update(dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccExtraCargos), validaTexto(False, txtExtraCharges, dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccExtraCargos)), validaTexto(True, txtExtraCharges, dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_idDiccExtraCargos)), publish)
                            End If
                        End With
                    End If
                    cInfoActual.IsSingleImgInv = chkSingleImgInv.Checked
                    sData = getDataXML(MyBase.cInfoActual.Hotel)
                    'me.cInfoActual.perfil = nuevo perfil
                    Me.guardalog("/HotelAdministrator/PagesGeneral/Hotel.aspx", If(publish, PaginaBase.acciones.Publicar, PaginaBase.acciones.Modificar), "Modificacion de los datos del hotel", "", sDataPrev, sData)
                    If Not publish Then Me.NotifyContentModification("Información del hotel", "Información General")
                End If
            End With
            MyBase.redirectTo(PaginaBase.pages.Home)
        End If
    End Sub

    
    Public Function validaTexto(ByVal bIdioma As Boolean, ByVal txtControlIdioma As CtrlIdioma, ByVal idDicc As Integer) As String
        Dim texto As String
        Dim auxControlIdioma As CtrlIdioma = New CtrlIdioma
        Dim auxTxtIng As String = ""
        Dim auxTxtEsp As String = ""

        auxControlIdioma.CargaDatosAuxiliares(idDicc, auxTxtIng, auxTxtEsp)

        If bIdioma = True Then 'valida Idioma Español
            If txtControlIdioma.GetES = Nothing Then
                texto = " "
            Else
                If txtControlIdioma.GetES.Trim = "" Then
                    texto = " "
                Else
                    texto = txtControlIdioma.GetES
                End If
            End If

            If texto = " " Then
                If auxTxtEsp = Nothing Then
                    texto = Nothing
                End If
            End If
        Else 'valida Idioma Ingles
            If txtControlIdioma.GetEN = Nothing Then
                texto = " "
            Else
                If txtControlIdioma.GetEN.Trim = "" Then
                    texto = " "
                Else
                    texto = txtControlIdioma.GetEN
                End If
            End If

            If texto = " " Then
                If auxTxtIng = Nothing Then
                    texto = Nothing
                End If
            End If
        End If

        Return texto
    End Function

    Private Function validaIdDicc(ByVal idDicc As Boolean, ByVal textoEsp As Object, ByVal textoIng As Object) As Boolean
        Dim valida As Boolean = False

        If idDicc = True And (textoEsp <> Nothing Or textoIng <> Nothing) Then
            If textoEsp <> Nothing Then
                If (textoEsp).Trim <> "" Then
                    valida = True
                End If
            End If

            If textoIng <> Nothing Then
                If (textoIng).Trim <> "" Then
                    valida = True
                End If
            End If
        End If

        Return valida
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        LoadResources()
    End Sub

    'Private Function htmlWriteCalendar()
    '    Dim iFrameCalendar As String = "<iframe width=""174"" height=""189"" name=""gToday:normal:agenda.js"" id=""gToday:normal:agenda.js"" src=""" & Request.ApplicationPath & "/Calendar/" & PortalCulture.GetCulture.Name.Substring(0, 2) & "/ipopeng.htm"" scrolling=""no"" frameborder=""0"" style=""Z-INDEX:999; LEFT:-500px; VISIBILITY:visible; POSITION:absolute; TOP:-500px""></iframe>"
    '    Page.RegisterStartupScript("frameCalendar", iFrameCalendar)
    'End Function

    'Private Function htmlSetCalendar(ByVal elementToRender As Literal, ByVal elementClientIdToGetSetDate As String)
    '    Dim htmlCal As String = "<a href=""javascript:void(0)"" onclick=""if(self.gfPop)gfPop.fPopCalendar(" & elementClientIdToGetSetDate & ");return false;"" HIDEFOCUS><img class=""PopcalTrigger"" align=""absMiddle"" src=""" & Request.ApplicationPath & "/Calendar/calbtn.gif"" border=""0"" alt=""""></a>"
    '    elementToRender.Text = htmlCal
    'End Function
    Private Sub CargaCorporativos()
        Dim ds As DataSet
        If MyBase.IsSupervisor Then
            With New HotelSistema
                ds = .GetCorporativos(0)
            End With
        Else
            With New HotelSistema
                ds = .GetCorporativos(CType(Me.Page, PaginaBase).UserIdentityName)
            End With
        End If
        ddlCorporativos.DataSource = ds
        ddlCorporativos.DataTextField = "NombreCorp"
        ddlCorporativos.DataValueField = "idCorporativo"
        ddlCorporativos.DataBind()
        If CType(Me.Page, PaginaBase).IsSupervisor() Then
            ddlCorporativos.Items.Insert(0, PortalCulture.GetString("M000482"))
            ddlCorporativos.Items(0).Value = 0
        End If

    End Sub

    Private Sub LoadResources()
        Me.lblCancel.Text = PortalCulture.GetString("00440", True)
        lblRules.Text = PortalCulture.GetString("M000589")
        Me.lblEmailLanguage.Text = PortalCulture.GetString("M000658", True)
        Me.lblMoneda.Text = PortalCulture.GetString("M0UT00477", True)
        lblTitle.Text = PortalCulture.GetString("00434")
        Me.lblMaxDiasRenta.Text = PortalCulture.GetString("M000588", True)
        Me.lblPlusTax.Text = PortalCulture.GetString("M000522", True)
        Me.lblImpuesto.Text = PortalCulture.GetString("M0UT00483") & " %:"    '"Impuesto"
        Me.lblEdadMaximaNiño.Text = PortalCulture.GetString("01322") & " < "
        Me.lblDiasAnticipados.Text = PortalCulture.GetString("00396", True)
        Me.lblDays.Text = PortalCulture.GetString("00397")
        Me.lblCheckout.Text = PortalCulture.GetString("M0UT00487", True)
        Me.lblCheckin.Text = PortalCulture.GetString("M0UT00488", True)
        Me.lblCategoria.Text = PortalCulture.GetString("M0UT00006", True)
        Me.lblAvlOnCorpModule.Text = PortalCulture.GetString("01618", True)
        Me.cmbCategoria.Items(0).Text = "1 " & PortalCulture.GetString("M0UT00493")    '"estrella"
        For i As Integer = 1 To 4
            Me.cmbCategoria.Items(i).Text = (i + 1) & " " & PortalCulture.GetString("M0UT00494")    '& "estrellas"
        Next
        Me.cmbCategoria.Items(5).Text = PortalCulture.GetString("00798")

        lblServiceCharge.Text = PortalCulture.GetString("M0UT02696", True)
        lblCommision.Text = PortalCulture.GetString("M0UT02697") & " %:"

        Me.lblEcotasaMessage.Text = PortalCulture.GetString("01665")
        Me.RangeValidatorTxtEcotasa.Text = PortalCulture.GetString("01666")

        lblNoArrivals.Text = PortalCulture.GetString("M000449")
        'RequiredFieldValidator11.Text = PortalCulture.GetString("M0UT02696") & " " & PortalCulture.GetString("M0UT02715")

        RequiredFieldValidator12.Text = PortalCulture.GetString("M0UT02697") & " " & PortalCulture.GetString("M0UT02715")
        Rangevalidator2.Text = "" '"Edad min. de Niño es numerico (1-99)"
        RangeValidator7.Text = PortalCulture.GetString("01168")    '"Edad Max. de Niño es numerico (1-99)"
        RequiredFieldValidator8.Text = PortalCulture.GetString("M0UT00502")    '"Impuesto es requerido"
        RangeValidator9.Text = PortalCulture.GetString("M0UT00503")    '"Impuesto es numerico (1-99)"
        Me.lblConfigGDS.Text = PortalCulture.GetString("M000288")
        Me.lblPropertyNumber.Text = PortalCulture.GetString("M000289", True)
        Me.lblChainCode.Text = PortalCulture.GetString("M000290", True)
        Me.lblGetRates.Text = PortalCulture.GetString("00542", True)
        Me.rbGetRates_True.Text = PortalCulture.GetString("M000159")
        Me.rbGetRates_False.Text = PortalCulture.GetString("M000160")
        '***
        Me.lblEmail.Text = PortalCulture.GetString("M000292", True)
        Me.lblLunes.Text = PortalCulture.GetString("M000300")
        Me.lblMartes.Text = PortalCulture.GetString("M000301")
        Me.lblMiercoles.Text = PortalCulture.GetString("M000302")
        Me.lblJueves.Text = PortalCulture.GetString("M000303")
        Me.lblViernes.Text = PortalCulture.GetString("M000304")
        Me.lblSabado.Text = PortalCulture.GetString("M000305")
        Me.lblDomingo.Text = PortalCulture.GetString("M000306")
        Me.lblEstanciaMin.Text = PortalCulture.GetString("M000587", True)
        Me.lblStatusA.Text = PortalCulture.GetString("M000308", True)
        Me.ddlStatus.Items(0).Text = (PortalCulture.GetString("M000309"))
        Me.ddlStatus.Items(1).Text = (PortalCulture.GetString("M000310"))
        Me.ddlStatus.Items(2).Text = (PortalCulture.GetString("M000311"))
        Me.lblError.Text = PortalCulture.GetString("M000314")
        Me.lblErrorDate.Text = PortalCulture.GetString("M000315")
        lblCancelPolitiesFull.Text = PortalCulture.GetString("M000536", True)
        lblCancelPolitiesReview.Text = PortalCulture.GetString("M000527", True)
        lblGuarantyPolicies.Text = PortalCulture.GetString("M000528", True)
        lblCreditCardPolicies.Text = PortalCulture.GetString("M000529", True)
        lblExtraCharges.Text = PortalCulture.GetString("M000530", True)
        lblIdGal.Text = PortalCulture.GetString("M000532", True)
        lblIdSabre.Text = PortalCulture.GetString("M000533", True)
        lblIdWorldSpan.Text = PortalCulture.GetString("M000534", True)
        lblIdAmadeus.Text = PortalCulture.GetString("M000535", True)
        Me.lblErrorPWorld.Text = PortalCulture.GetString("M000537")
        Me.lblErrorPSabre.Text = PortalCulture.GetString("M000537")
        Me.lblErrorPGalileo.Text = PortalCulture.GetString("M000537")
        Me.lblErrorPAmadeus.Text = PortalCulture.GetString("M000537")
        btnSave.Text = PortalCulture.GetString("M000106")
        ddlCancelationPolicy.Items(cancelpolicy.bydays).Text = PortalCulture.GetString("00020")
        ddlCancelationPolicy.Items(cancelpolicy.byhour).Text = PortalCulture.GetString("00021")
        ddlCancelationPolicy.Items(cancelpolicy.specifichour).Text = PortalCulture.GetString("00381")
        lblConfirmationEmail.Text = PortalCulture.GetString("00414")
        lbltitlepolity.Text = PortalCulture.GetString("00446")
        lblMaxCuartos.Text = PortalCulture.GetString("00493", True)
        lblErrorMail.Text = PortalCulture.GetString("00356")
        Me.lblFaxEmail.Text = PortalCulture.GetString("00494", True)
        Me.lblPerfil.Text = PortalCulture.GetString("00495", True)
        Me.lblAvailOnPortal.Text = PortalCulture.GetString("00526", True)
        Me.lblAvailOnOnePage.Text = PortalCulture.GetString("00527", True)
        Me.lblAvailOnGDS.Text = PortalCulture.GetString("00525", True)
        Me.lblAvailOnADS.Text = PortalCulture.GetString("01013", True)
        Me.lblAmhm.Text = PortalCulture.GetString("00676", True)
        lblEmprTour.Text = PortalCulture.GetString("00677", True)
        Me.lblAllowDeposit.Text = PortalCulture.GetString("00681", True)
        lblCadena.Text = PortalCulture.GetString("00838", True)
        lblEsmoroso.Text = PortalCulture.GetString("01017", True)
        lblEsPagoCero.Text = PortalCulture.GetString("01160", True)

        lblEdadNinio.Text = PortalCulture.GetString("01324")
        lblJuniorAnios.Text = PortalCulture.GetString("01324")
        lblNoCobrarAnios.Text = PortalCulture.GetString("01324")
        lblEdadMinimaNiño.Text = PortalCulture.GetString("01323") & " < "
        lblMinCuartos.Text = PortalCulture.GetString("01165", True)
        lblMinOcupacion.Text = PortalCulture.GetString("01166", True)
        lblMaxOcupacion.Text = PortalCulture.GetString("01167", True)
        Me.chkTransUp.Text = PortalCulture.GetString("01180")
        lblEdadMaximaAdo.Text = PortalCulture.GetString("01277") & " < "
        rvEdadaAdolecente.Text = PortalCulture.GetString("01278")
        rfvEdadNinio.Text = PortalCulture.GetString("00071")
        lblErrorEdadNoCobrar.Text = PortalCulture.GetString("01331")
        lblErrorEdadNinio.Text = PortalCulture.GetString("01332")
        Me.btnPublsh.Text = PortalCulture.GetString("01364")
        lblLatitud.Text = PortalCulture.GetString("01452", True)
        lblLongitud.Text = PortalCulture.GetString("01453", False)
        lblSaveCurrencyShonw.Text = PortalCulture.GetString("01648", True)

        If IsSupervisor Then
            lblSingleImgInv.Visible = True
            chkSingleImgInv.Visible = True
            chkPushNotif.Visible = True
            lblPushNotif.Visible = True
        End If
    End Sub

    Private Function saveDeparture() As String
        Dim DepRes As String
        DepRes = ""
        If L.Checked Then
            DepRes = "Y"
        Else
            DepRes = "N"
        End If
        If Ma.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        If Mi.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        If J.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        If V.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        If S.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        If D.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        Return DepRes
    End Function


#End Region

    Private Sub cmbMonedas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonedas.SelectedIndexChanged
        Dim dsEtiq As MonedaDatos

        dsEtiq = (New MonedaSistema).GetMonedaById(cmbMonedas.SelectedValue)
        Me.Label2.Text = dsEtiq.Tables(dsEtiq.MONEDA_TABLE).Rows(0).Item(dsEtiq.FIELD_ABREVIATURA)

    End Sub

    Private Function validaHours() As Boolean
        Dim valido As Boolean = True
        If Me.ddlCancelationPolicy.SelectedIndex <> Me.cancelpolicy.specifichour Then
            If txtCancellationPolicy.Text.Length > 3 Then
                valido = False
            End If
            lblErrorHours.Text = "0-999"
        End If
        lblErrorHours.Visible = Not valido
        Return valido
    End Function

    Function ValidaEdad(ByVal edad1 As String, ByVal edad2 As String) As Boolean
        Dim valor As Integer
        Dim valor1 As Integer

        Integer.TryParse(edad1, valor)
        Integer.TryParse(edad2, valor1)
        If valor <> 0 Then
            If (valor > valor1) Then
                Return False
            End If
        End If
        Return True
    End Function


    Private Function validaMail() As Boolean
        lblErrorMail.Visible = False
        Dim reg As System.Text.RegularExpressions.Regex
       
        For Each st As String In txtEmailReservas.Text.Trim.Split(",")
            If Not reg.IsMatch(st, "^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$") Then
                lblErrorMail.Visible = True
                Return False
            End If
        Next

        If Not ValidaEdad(txtEdadMinimaNino.Text, txtEdadMaximaNino.Text) Then
            lblErrorEdadNoCobrar.Visible = True
            Return False
        End If

        Dim valor As Integer
        Integer.TryParse(txtEdadMaximaAdo.Text, valor)
        If (valor > 0) Then
            If Not ValidaEdad(txtEdadMaximaNino.Text, txtEdadMaximaAdo.Text) Then
                lblErrorEdadNinio.Visible = True
                Return False
            End If
        End If

        Return True
    End Function

    Private Function BuildDataImpuestos() As DataSet
        Dim ds As DataSet
        Dim table As DataTable = New DataTable("Table1")

        ds = New DataSet
        With table.Columns
            .Add("idHotel", GetType(System.Int32))
            .Add("impuesto", GetType(System.Decimal))
            .Add("impuestoNuevo", GetType(System.Decimal))
        End With
        ds.Tables.Add(table)
        Return ds
    End Function


End Class
