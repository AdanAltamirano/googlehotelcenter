Imports Portal.General.Facade
Imports Portal.General.Common
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Partial Class HotelAsosiationCars
    Inherits PaginaBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmbMonedas As System.Web.UI.WebControls.DropDownList
    
    Protected WithEvents chkFranquiciasFist1 As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents chkFranquiciasFist2 As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents chkFranquiciasFist3 As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents chkFranquiciasFist4 As System.Web.UI.WebControls.CheckBoxList

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
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not Me.IsPostBack Then
            CargaCorporativos()
            loadData()

        End If
        Idiomas()
    End Sub

    Private Sub mensaje(ByVal mensaje As String)
        Trace.Warn(mensaje)
    End Sub

    Private Property IdChainCode() As String
        Get
            Return ViewState("IdChainCode")
        End Get
        Set(ByVal Value As String)
            ViewState("IdChainCode") = Value
        End Set
    End Property

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



    End Sub
    Private Sub loadFranquicias(ByVal Chaincode As String)
        Try
            Dim SearchCar As New SearchEngine.CarRents.Facade.BookingAutosFacade
            Dim Franquicias As DataSet
            'Franquicias = SearchCar.SearchAutosFranquiciasList

            Dim valor = Franquicias.Tables(0).Rows.Count - 1 / 4

            Dim item As WebControls.ListItem

            chkFranquicias1.Items.Clear()
            chkFranquicias2.Items.Clear()
            chkFranquicias3.Items.Clear()
            chkFranquicias4.Items.Clear()

            For indice As Integer = 0 To 19
                Try
                    item = New WebControls.ListItem
                    item.Text = Franquicias.Tables(0).Rows(indice)("Descripcion")
                    item.Value = Franquicias.Tables(0).Rows(indice)("Codigo")
                    chkFranquicias1.Items.Add(item)
                Catch ex As Exception

                End Try

            Next

            For indice As Integer = 20 To 39
                Try
                    item = New WebControls.ListItem
                    item.Text = Franquicias.Tables(0).Rows(indice)("Descripcion")
                    item.Value = Franquicias.Tables(0).Rows(indice)("Codigo")
                    chkFranquicias2.Items.Add(item)
                Catch ex As Exception

                End Try
            Next

            For indice As Integer = 40 To 59
                Try
                    item = New WebControls.ListItem
                    item.Text = Franquicias.Tables(0).Rows(indice)("Descripcion")
                    item.Value = Franquicias.Tables(0).Rows(indice)("Codigo")
                    chkFranquicias3.Items.Add(item)
                Catch ex As Exception

                End Try
            Next

            For indice As Integer = 60 To 200
                Try
                    item = New WebControls.ListItem
                    item.Text = Franquicias.Tables(0).Rows(indice)("Descripcion")
                    item.Value = Franquicias.Tables(0).Rows(indice)("Codigo")
                    chkFranquicias4.Items.Add(item)
                Catch ex As Exception
                    Exit For
                End Try
            Next



            Dim FR As DataSet
            'FR = SearchCar.SearchAutosFranquiciasCorporativoList(Chaincode)

            If Not FR Is Nothing AndAlso FR.Tables(0).Rows.Count > 0 Then


                For indice As Integer = 0 To chkFranquicias1.Items.Count - 1
                    If FR.Tables("Booking").Select("Franquicia ='" + chkFranquicias1.Items(indice).Value.ToString() + "'").Length > 0 Then
                        chkFranquicias1.Items(indice).Selected = True
                    End If
                Next

                For indice As Integer = 0 To chkFranquicias2.Items.Count - 1

                    If FR.Tables("Booking").Select("Franquicia ='" + chkFranquicias2.Items(indice).Value.ToString() + "'").Length > 0 Then
                        chkFranquicias2.Items(indice).Selected = True
                    End If
                Next

                For indice As Integer = 0 To chkFranquicias3.Items.Count - 1

                    If FR.Tables("Booking").Select("Franquicia ='" + chkFranquicias3.Items(indice).Value.ToString() + "'").Length > 0 Then
                        chkFranquicias3.Items(indice).Selected = True
                    End If
                Next

                For indice As Integer = 0 To chkFranquicias4.Items.Count - 1

                    If FR.Tables("Booking").Select("Franquicia ='" + chkFranquicias4.Items(indice).Value.ToString() + "'").Length > 0 Then
                        chkFranquicias4.Items(indice).Selected = True
                    End If
                Next
            Else
                Try
                    mensaje(" (loadFranquicias) -- Relaciones -- Respuesta" + FR.GetXml)
                Catch ex As Exception
                    mensaje(" (loadFranquicias) -- Relaciones --)")
                End Try

            End If
        Catch ex As Exception
            mensaje(" (loadFranquicias) " + ex.Message)
        End Try


    End Sub

    Private Sub loadData()
        Dim dsHotel As HotelDatos
        Dim dsEtiq As MonedaDatos

        With New HotelSistema
            dsHotel = .GetHotelById(MyBase.cInfoActual.Hotel)
        End With

        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)

                Try
                    If Not .IsNull(HotelDatos.fld_idcorporativo) Then
                        ddlCorporativos.SelectedValue = .Item(HotelDatos.fld_idcorporativo)
                        ' ddlCorporativos.Enabled = False
                        Me.IdChainCode = ddlCorporativos.SelectedValue
                        loadFranquicias(ddlCorporativos.SelectedItem.Text)
                    End If
                Catch ex As Exception
                    mensaje(" (loadData) " + ex.Message)
                End Try

            End With
        End If
    End Sub

    Private Sub AltaRelacion(ByVal Chaincode As String)
        Try
            Dim SearchCar As New SearchEngine.CarRents.Facade.BookingAutosFacade
            For indice As Integer = 0 To chkFranquicias1.Items.Count - 1
                If chkFranquicias1.Items(indice).Selected Then
                    'SearchCar.SearchAutosFranquiciasCorporativoInsert(Chaincode, chkFranquicias1.Items(indice).Value, 1)
                Else
                    'SearchCar.SearchAutosFranquiciasCorporativoInsert(Chaincode, chkFranquicias1.Items(indice).Value, 0)
                End If
            Next


            For indice As Integer = 0 To chkFranquicias2.Items.Count - 1
                If chkFranquicias2.Items(indice).Selected Then
                    'SearchCar.SearchAutosFranquiciasCorporativoInsert(Chaincode, chkFranquicias2.Items(indice).Value, 1)
                Else
                    'SearchCar.SearchAutosFranquiciasCorporativoInsert(Chaincode, chkFranquicias2.Items(indice).Value, 0)
                End If
            Next

            For indice As Integer = 0 To chkFranquicias3.Items.Count - 1
                If chkFranquicias3.Items(indice).Selected Then
                    'SearchCar.SearchAutosFranquiciasCorporativoInsert(Chaincode, chkFranquicias3.Items(indice).Value, 1)
                Else
                    'SearchCar.SearchAutosFranquiciasCorporativoInsert(Chaincode, chkFranquicias3.Items(indice).Value, 0)
                End If
            Next

            For indice As Integer = 0 To chkFranquicias4.Items.Count - 1
                If chkFranquicias4.Items(indice).Selected Then
                    'SearchCar.SearchAutosFranquiciasCorporativoInsert(Chaincode, chkFranquicias4.Items(indice).Value, 1)
                Else
                    'SearchCar.SearchAutosFranquiciasCorporativoInsert(Chaincode, chkFranquicias4.Items(indice).Value, 0)
                End If
            Next
        Catch ex As Exception
            mensaje(" (loadData) " + ex.Message)
        End Try
    End Sub
    Private Sub CmdAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdAceptar.Click
        AltaRelacion(IdChainCode)
    End Sub

    Private Sub cmdCargar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCargar.Click
        Me.IdChainCode = ddlCorporativos.SelectedValue
        loadFranquicias(ddlCorporativos.SelectedItem.Text)
    End Sub

    Private Sub Idiomas()
        Me.LnkSelectAll.InnerText = PortalCulture.GetString("00484")
        Me.LnkUnSelectAll.InnerText = PortalCulture.GetString("00876")
        Me.lbltitleCar.Text = PortalCulture.GetString("00877")
        Me.lblCategoria.Text = PortalCulture.GetString("00838", True)
        Me.cmdCargar.Text = PortalCulture.GetString("00149")
        Me.CmdAceptar.Text = PortalCulture.GetString("M000106")
        Me.lblTitle.Text = PortalCulture.GetString("00878")
    End Sub
End Class
