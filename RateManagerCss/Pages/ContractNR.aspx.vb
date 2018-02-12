Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade

Partial Class ContractNR
    Inherits PaginaBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Protected WithEvents CtlMensajes1 As ctlMensajes
    Enum dgcolumns
        Nombre
        idContrato
        Editar
        Eliminar
    End Enum
    Private Property idContrato() As Integer
        Get
            Return viewstate("idContrato")
        End Get
        Set(ByVal Value As Integer)
            viewstate("idContrato") = Value
        End Set
    End Property

    Sub MostrarCmdNew(ByVal show As Boolean)
        cmdNew.Style.Add("display", IIf(show, "block", "none"))
        divContenedor.Style.Add("display", IIf(show, "none", "block"))
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            Me.idContrato = -1
            LoadContracts()
            MostrarCmdNew(True)
        End If
        Me.ResizefrmPrincipal()
        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", divContenedor.ClientID, cmdNew.ClientID, "true"))
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.btnAceptar.Text = PortalCulture.GetString("00008")
        Me.btnCancel.Text = PortalCulture.GetString("00009")
        cmdNew.Value = PortalCulture.GetString("00102")
        lblTitle.Text = PortalCulture.GetString("01114", False)
        lblNombre.Text = PortalCulture.GetString("01115", True)
        lblPorcMinimo.Text = PortalCulture.GetString("01116", True)
        lblPorcMaximo.Text = PortalCulture.GetString("01117", True)
        lblMsgErrorNombre.Text = PortalCulture.GetString("01118", False)
        lblMsgErrorMinimo.Text = PortalCulture.GetString("01119", False)
        lblMsgErrorMaximo.Text = PortalCulture.GetString("01120", False)
    End Sub
#Region "Metodos"
    Private Function LoadContracts()
        Dim Contratos As ContractNetRateData
        With (New ContractNetRateFacade)
            Contratos = .LoadContractsByIdHotel(Me.cInfoActual.Hotel)
        End With
        If Not Contratos Is Nothing AndAlso Contratos.Tables(ContractNetRateData.CONTRACTNR_TABLE).Rows.Count > 0 Then
            dgcontract.DataSource = Contratos.Tables(ContractNetRateData.CONTRACTNR_TABLE)
            dgcontract.DataBind()
            dgcontract.Visible = True
        Else
            'No se encontraron contratos
            dgcontract.Visible = False
        End If
    End Function

    Private Function EliminarContrato() As Boolean
        Dim banErr As Boolean = False
        Dim banErr2 As Boolean = False
        With (New ContractNetRateFacade)
            'lblMsgActualizacion.Visible = True
            'Verificamos que no tenga un RatePlan ASociado
            If .ExistsContractNRRatePlan(Me.idContrato, Me.cInfoActual.Hotel) = False Then
                If .DelContractNetRate(Me.idContrato, Me.cInfoActual.Hotel) Then
                    'Desplegamos el mensaje
                    'lblMsgActualizacion.Text = "Se Elimino el cotrato"
                    If dgcontract.CurrentPageIndex > 0 And dgcontract.Items.Count = 1 Then
                        dgcontract.CurrentPageIndex = ((dgcontract.CurrentPageIndex * dgcontract.PageSize) \ dgcontract.PageSize) - 1
                    End If
                    dgcontract.SelectedIndex = -1
                    'lblMsgActualizacion.Text = "Se elimino el Contrato"
                Else
                    'Mensaje De error
                    lblMsgActualizacion.Text = "No se logro eliminar el contrato"
                    banErr = True
                End If
            Else
                lblMsgActualizacion.Text = PortalCulture.GetString("01123")
                banErr2 = True
                banErr = True
            End If
        End With
        LimpiarControles()
        If banErr Then
            'lblMsgActualizacion.Visible = True
        End If
        If banErr2 Then
            lblMsgActualizacion.Visible = True
        End If
        LoadContracts()
    End Function
#End Region

    Private Sub dgContractNR_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgcontract.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            Dim LK As HyperLink
            Dim LK2 As LinkButton
            LK = e.Item.Cells(dgcolumns.Eliminar).FindControl("lnkEliminar")
            LK.Text = PortalCulture.GetString("00103")
            LK2 = e.Item.Cells(dgcolumns.Eliminar).FindControl("lnkedit")
            LK2.Text = PortalCulture.GetString("00093")
            LK2 = e.Item.Cells(dgcolumns.Eliminar).FindControl("lnkEliminar2")
            LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("01114"), _
            PortalCulture.GetString("01137"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Nombre).Text = PortalCulture.GetString("00073")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            'e.Item.Cells(dgcolumns.eliminar).Text = CType(dgRatePlans.DataSource, DataSet).Tables(RatePlanData.RATEPLAN_TABLE).Rows.Count & " " & PortalCulture.GetString("00047")
        End If
    End Sub

    Private Sub dgContractNR_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgcontract.ItemCommand
        Call LimpiarControles()

        If e.CommandName = "Select" Then
            Try
                Me.idContrato = CType(e.Item.Cells(dgcolumns.idContrato).Text, Integer)
                Dim contrato As ContractNetRateData
                With (New ContractNetRateFacade)
                    contrato = .getContractByIdContract(Me.idContrato, Me.cInfoActual.Hotel)
                End With
                TextBoxNombre.Text = contrato.Tables(ContractNetRateData.CONTRACTNR_TABLE).Rows(0)(ContractNetRateData.FIELD_NOMBRE)
                TextBoxPorcMax.Text = contrato.Tables(ContractNetRateData.CONTRACTNR_TABLE).Rows(0)(ContractNetRateData.FIELD_PORCENTAJEMAXIMO)
                TextBoxPorcMin.Text = contrato.Tables(ContractNetRateData.CONTRACTNR_TABLE).Rows(0)(ContractNetRateData.FIELD_PORCENTAJEMINIMO)
                lblMsg.Text = PortalCulture.GetString("01460")

            Catch ex As Exception
                Me.idContrato = -1
                Exit Sub
            End Try            
            MostrarCmdNew(False)
        ElseIf e.CommandName = "Eliminar" Then
            Try
                Me.idContrato = CType(e.Item.Cells(dgcolumns.idContrato).Text, Integer)
                Call EliminarContrato()
                MostrarCmdNew(False)
            Catch ex As Exception
                Me.idContrato = -1
                Exit Sub
            End Try

        End If
    End Sub
    Private Sub LimpiarControles()
        TextBoxNombre.Text = ""
        TextBoxPorcMax.Text = ""
        TextBoxPorcMin.Text = ""
        Me.idContrato = -1
        lblMsgErrorMaximo.Visible = False
        lblMsgErrorMinimo.Visible = False
        lblMsgErrorNombre.Visible = False
        lblMsgActualizacion.Visible = False
        lblMsg.Text = PortalCulture.GetString("01459")
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        'Validamos los valores de los campos
        lblMsgErrorMaximo.Visible = False
        lblMsgErrorMinimo.Visible = False
        lblMsgErrorNombre.Visible = False
        Dim PorcMinimo As Integer = 0
        Dim PorcMaximo As Integer = 0
        Dim sData As String = ""
        Dim sDataPrev As String = ""

        If TextBoxNombre.Text.Trim = String.Empty Then
            lblMsgErrorNombre.Visible = True
            MostrarCmdNew(False)
            Exit Sub

        End If

        Dim dsc As ContractNetRateData
        With (New ContractNetRateFacade)
            dsc = .getContractByIdContract(Me.idContrato, Me.cInfoActual.Hotel)
        End With

        If Me.idContrato <> -1 Then
            sDataPrev = Util.Utility.GetXml(dsc.CONTRACTNR_TABLE, "UpdateContratosNR", dsc)
        End If

        Try
            If TextBoxPorcMin.Text.Trim <> String.Empty Then
                PorcMinimo = CType(TextBoxPorcMin.Text.Trim, Integer)
                If PorcMinimo < 0 Or PorcMinimo > 100 Then
                    'Desplegamos mensaje de valor invalido
                    lblMsgErrorMinimo.Visible = True                    
                    Exit Sub
                End If
            Else
                lblMsgErrorMinimo.Visible = True
                MostrarCmdNew(False)
                Exit Sub
            End If
        Catch ex As Exception
            'Desplegamos el mensaje de valor invalido
            lblMsgErrorMinimo.Visible = True
            Exit Sub
        End Try

        Try
            If TextBoxPorcMax.Text.Trim <> String.Empty Then
                PorcMaximo = CType(TextBoxPorcMax.Text.Trim, Integer)
                If PorcMaximo < 0 Or PorcMaximo > 100 Then
                    lblMsgErrorMaximo.Visible = True
                    Exit Sub
                End If
            Else
                lblMsgErrorMaximo.Visible = True
                MostrarCmdNew(False)
                Exit Sub
            End If
        Catch ex As Exception
            lblMsgErrorMaximo.Visible = True
            Exit Sub
        End Try

        If PorcMinimo < 0 Or PorcMinimo > 100 Or PorcMinimo > PorcMaximo Then
            'Desplegamos mensaje de valor invalido
            lblMsgErrorMinimo.Visible = True
            Exit Sub
        End If


        If Me.idContrato = -1 Then
            'Agregamos Contrato
            With (New ContractNetRateFacade)
                If .AddContractNetRate(Me.cInfoActual.Hotel, TextBoxNombre.Text, PorcMaximo, PorcMinimo, Nothing, Nothing, Nothing) Then
                    'Desplegamos el mensaje de agregado.
                    dsc = New ContractNetRateData
                    Dim dr As DataRow
                    dr = dsc.Tables(dsc.CONTRACTNR_TABLE).NewRow
                    dr(dsc.FIELD_IDHOTEL) = Me.cInfoActual.Hotel
                    dr(dsc.FIELD_NOMBRE) = TextBoxNombre.Text
                    dr(dsc.FIELD_PORCENTAJEMAXIMO) = PorcMaximo
                    dr(dsc.FIELD_PORCENTAJEMINIMO) = PorcMinimo
                    dsc.Tables(dsc.CONTRACTNR_TABLE).Rows.Add(dr)
                    dsc.AcceptChanges()
                    sData = Util.Utility.GetXml(dsc.CONTRACTNR_TABLE, "UpdateContratosNR", dsc)
                    ' lblMsgActualizacion.Text = PortalCulture.GetString("01121")                    
                    MostrarCmdNew(True)
                    dgcontract.SelectedIndex = -1
                Else
                    'Desplegamos el mensaje de error
                    lblMsgActualizacion.Text = "Ocurrio un error al agregar el contrato"
                    'lblMsgActualizacion.Visible = True
                    Exit Sub
                End If
            End With
        Else
            'Actualizamos Contrato
            With (New ContractNetRateFacade)
                If .UpdContractNR(Me.idContrato, Me.cInfoActual.Hotel, TextBoxNombre.Text, PorcMaximo, PorcMinimo, Nothing, Nothing, Nothing) Then
                    'Desplegamos el mensaje de agregado.
                    With (New ContractNetRateFacade)
                        dsc = .getContractByIdContract(Me.idContrato, Me.cInfoActual.Hotel)
                    End With
                    sData = Util.Utility.GetXml(dsc.CONTRACTNR_TABLE, "UpdateContratosNR", dsc)
                    '   lblMsgActualizacion.Text = PortalCulture.GetString("01122")
                    Me.guardalog("/Pages/ContractNr.aspx", PaginaBase.acciones.Modificar, "Se Modificó el contrato NR  " & TextBoxNombre.Text, "", sDataPrev, sData)
                    MostrarCmdNew(True)
                    dgcontract.SelectedIndex = -1
                Else
                    'Desplegamos el mensaje de agregado.
                    lblMsgActualizacion.Text = "Ocurrio un error al actualizar el contrato"
                    'lblMsgActualizacion.Visible = True
                    Me.guardalog("/Pages/ContractNr.aspx", PaginaBase.acciones.Crear, "Se creo el contrato NR " & TextBoxNombre.Text, "", sDataPrev, sData)
                    Exit Sub
                End If
            End With
        End If
        Call LimpiarControles()
        'lblMsgActualizacion.Visible = True
        Call LoadContracts()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        LimpiarControles()
        LoadContracts()
        dgcontract.SelectedIndex = -1
        MostrarCmdNew(True)
    End Sub
    'Private Sub btnCancel2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
    '    LimpiarControles()
    '    LoadContracts()
    '    MostrarCmdNew(True)
    'End Sub

    Private Sub dgContractNR_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgcontract.PageIndexChanged
        dgcontract.CurrentPageIndex = e.NewPageIndex
        dgcontract.SelectedIndex = -1
        LoadContracts()
    End Sub
End Class
