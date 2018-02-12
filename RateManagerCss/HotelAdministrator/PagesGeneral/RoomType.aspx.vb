Imports Portal.Hotel.Common.Data

Partial Class RoomType
    Inherits paginabase
    Protected WithEvents CtlMensajes1 As ctlMensajes
#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents CtrlRoomType1 As ctrlRoomType

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
        If Not IsPostBack Then
            Me.lnkDelete.NavigateUrl = CtlMensajes1.getShow(lnkDelete2.ClientID, PortalCulture.GetString("00056"), PortalCulture.GetString("00459"))
            laodRoomsTypes()
            lblError.Visible = False
        End If
        Me.ResizefrmPrincipal()
    End Sub
    Private Sub laodRoomsTypes()
        With CtrlRoomType1
            lstRoomTypes.DataValueField = RoomsTypeData.FLD_ID_ROOM_TYPE
            lstRoomTypes.DataTextField = RoomsTypeData.FLD_NAME
            lstRoomTypes.DataSource = .getAllRoomsTypes()
            lstRoomTypes.DataBind()
        End With
    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click
        CtrlRoomType1.newRoomType()
        lblError.Visible = False
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If CtrlRoomType1.deleteRoomType() Then
            laodRoomsTypes()
        Else
            lblError.Visible = False
        End If
    End Sub

    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        lblError.Visible = False
        If CtrlRoomType1.SaveRoomType() Then
            laodRoomsTypes()
        Else
            lblError.Visible = True
            lblError.Text = CtrlRoomType1.sError
        End If
    End Sub

    Private Sub loadResources()
        lblTitulo.Text = PortalCulture.GetString("M000032")
        lblRoom.Text = PortalCulture.GetString("00072", True)
        btnNuevo.Text = PortalCulture.GetString("M000058")
        lnkDelete.Text = PortalCulture.GetString("M000059")
        lnkEdit.Text = PortalCulture.GetString("00093")
        btnGuardar.Text = PortalCulture.GetString("M000060")        
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
    End Sub


    Private Sub lnkEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkEdit.Click
        CtrlRoomType1.loadRoomType(Me.lstRoomTypes.SelectedItem.Value)
        lblError.Visible = False
    End Sub

    Private Sub lnkDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDelete2.Click
        lblError.Visible = False
        If Not CtrlRoomType1.deleteRoomType(Me.lstRoomTypes.SelectedItem.Value) Then
            lblError.Visible = True
        End If
        laodRoomsTypes()
    End Sub

End Class
