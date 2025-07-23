Imports System.Drawing

Imports Portal.Hotel.Facade
Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports System.Configuration.ConfigurationManager
Imports Portal.Hotel.Common.Data
Imports System.Data.SqlClient
Imports Oz.UniBilling.Hotels.Business

Partial Class Welcome1
    Inherits PaginaBase
    Private Enum dgcolumns
        NoReservacion
        Hotel
        Cliente
        checkIn
        cantidad
        ID
        pmsACT
        details
        verificar
        NoReservacionDato
        idHotel
        IsNetRateUv
        deposittarget
    End Enum

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents hplListFares As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblFechaConciliar As System.Web.UI.WebControls.Label
    Protected WithEvents lblListFares As System.Web.UI.WebControls.Label
    Protected WithEvents lblNuevo As System.Web.UI.HtmlControls.HtmlGenericControl
    'Protected WithEvents imgCertificado As System.Web.UI.WebControls.ImageButton
    Protected WithEvents tdCertificado As HtmlTableRow
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
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
        Me.Page.RegisterStartupScript("loadHotel", "<script>loadInfoHotel();</script>")
        If MyBase.cInfoActual.Hotel = 0 Then
            If MyBase.IsSupervisor Or Me.IsUnibilling Or MyBase.IsContent Or MyBase.IsUsuarioCallCenter Then
                MyBase.redirectTo(PaginaBase.pages.SearchHotel)
            ElseIf MyBase.IsUsuarioHotel Then
                MyBase.redirectTo(PaginaBase.pages.SearchHotel)
                'clearviewstatepermisos()
                'Me.loadHotelUsuarioHotel()
                'If Not IsPostBack Then
                '    Session("urlCurrent") = Request.ApplicationPat & "/Portal/Pages/Welcome.aspx"
                'End If
            ElseIf IsAgencyCompany Then
                redirectTo(pages.ReservationListUI)
            Else
                loadHotels()
                loadData()
                SearchByDates()
            End If
        ElseIf MyBase.IsUsuarioCallCenter Then
            MyBase.redirectTo(PaginaBase.pages.WaitList)
        Else


            If Not IsPostBack Then
                dgDepositos.CurrentPageIndex = 0
                SearchByDates()
                If Not Request.UrlReferrer Is Nothing AndAlso Request.UrlReferrer.AbsoluteUri.IndexOf("Logon.aspx") > 0 Then
                    Me.guardalog("/Portal/Pages/Welcome.aspx", PaginaBase.acciones.LogIn, "Han Accesado al sistema")
                End If

            End If
            'SearchByDates()
            loadData()

            Session("urlCurrent") = Me.GeRequestApplicationPath("/Portal/Pages/Welcome.aspx")
            hplLIsting.NavigateUrl = UrlPage(PaginaBase.pages.ReservationList)
            hplVerifyRes.NavigateUrl = UrlPage(PaginaBase.pages.ConfirmReservas)
            hplStatus.NavigateUrl = UrlPage(PaginaBase.pages.HotelStatus)
            hplInventory.NavigateUrl = UrlPage(pages.InventarioHotel)
            If (New AuthUser).IsUsuarioHotelNetRate And (Not (New AuthUser).IsUsuarioHotelAvanzado) Then
                hplFares.Visible = False
                lblFares.Visible = False
            Else
                lblFares.Visible = True
                hplFares.Visible = True
                hplFares.NavigateUrl = UrlPage(PaginaBase.pages.FaresCatalogue)
            End If

            'hplListFares.NavigateUrl = UrlPage(PaginaBase.pages.ratechart)
            Me.hplResFrontDesk.NavigateUrl = GeRequestApplicationPath("/HotelAdministrator/Pages/PassiveBooking.aspx")

            hplFacturacion.NavigateUrl = GeRequestApplicationPath("/HotelAdministrator/Invoicing/Invoices.aspx")
            'hplFacturacion.NavigateUrl = GeRequestApplicationPath("/HotelAdministrator/Invoicing/InvoicesToConciliate.aspx")
            hplFrontDeskRes.NavigateUrl = GeRequestApplicationPath(String.Concat("/Documentacion/PDF/FrontDeskreservations_", PortalCulture.GetCulture.Name.Substring(0, 2), ".pdf"))

            hplLogoCertificacion.NavigateUrl = GeRequestApplicationPath(String.Concat("/Pages/Certificate.aspx?nombre=", Me.cInfoActual.HotelName))
            hplCertificacion.NavigateUrl = "https://crs.univisit.com/infoEmpresa/Default.aspx?np=" & Me.cInfoActual.Empresa
        End If

        'If Not PermisionContentWelcome("welcome.aspx") Then
        '    pnlReservas.Style.Add("display", "none")
        'End If
  
    End Sub

    Public Const PRM_ID_HOTEL As String = "@idHotel"
    Public Const PRM_CHECKIN As String = "@checkIn"
    Public Const PRM_CHECKOUT As String = "@checkOut"
    Public Const PRM_NAME_CLIENT As String = "@NameClient"
    Public Const PRM_FILTER As String = "@filter"
    Public Const PRM_IDIOMA As String = "@idioma"
    Public Const PRM_TARGET As String = "@DepositTarget"
    Public Const PRM_ISNETUV As String = "@isNetUV"
    Public Const PRM_IDUSUARIO As String = "@idUsuario"
    Public Const PRM_IDASOCIACION As String = "@idAsociacion"

    Public Const PRM_NORESERVATION As String = "@NoReservation"

    Private Sub SearchByDates()
        Dim ds As New DataSet
        ' Dim dv As DataView
        'Dim fecha = CDate(Me.txtInicio.Text)
        Dim ConnectionString As String = AppSettings("HotelConnectionString")
        Dim dsCommand As New SqlDataAdapter
        Dim IdAsociation As Integer

        If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), IdAsociation)
        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "spReservationsByDeposit_GetByDates"
                    .Connection = New SqlConnection(ConnectionString)
                    'If filtro > 0 Then
                    '    .Parameters.Add(New SqlParameter(PRM_FILTER, SqlDbType.Int)).Value = CInt(filtro)
                    '    .Parameters.Add(New SqlParameter(PRM_CHECKIN, SqlDbType.DateTime)).Value = Format(fecha1, "yyyy/MM/dd")
                    '    .Parameters.Add(New SqlParameter(PRM_CHECKOUT, SqlDbType.DateTime)).Value = Format(fecha2, "yyyy/MM/dd")
                    'End If
                    .Parameters.Add(New SqlParameter(PRM_NAME_CLIENT, SqlDbType.NVarChar, 80)).Value = ""
                    .Parameters.Add(New SqlParameter(PRM_IDIOMA, SqlDbType.Int)).Value = PortalCulture.GetIDCulture

                    If MyBase.isUserChain Then
                        .Parameters.Add(New SqlParameter(PRM_ID_HOTEL, SqlDbType.Int)).Value = MyBase.cInfoActual.Hotel
                        .Parameters.Add(New SqlParameter(PRM_IDUSUARIO, SqlDbType.Int)).Value = CType(Session("idUsuario"), Integer)                        
                    Else
                        .Parameters.Add(New SqlParameter(PRM_ID_HOTEL, SqlDbType.Int)).Value = MyBase.cInfoActual.Hotel
                    End If

                    If IdAsociation > 0 Then
                        .Parameters.Add(New SqlParameter(PRM_IDASOCIACION, SqlDbType.Int)).Value = IdAsociation
                    End If

                    If Not MyBase.IsSupervisor Then
                        '.Parameters.Add(New SqlParameter(PRM_TARGET, SqlDbType.NVarChar, 3)).Value = "HTL"
                    End If

                    'If (MyBase.IsSupervisor Or (MyBase.IsUsuarioHotelAssociation And MyBase.IdAsociation = 1)) Then
                    'Else
                    '    .Parameters.Add(New SqlParameter(PRM_ISNETUV, SqlDbType.TinyInt)).Value = 0
                    'End If


                End With
                .Fill(ds)
            Catch ex As Exception
                Dim s As String = ex.Message.ToString
                ds = Nothing 'TODO VS2008
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()
            End Try
        End With

        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)


        'obtener depositos cryptomoneda
        Dim dsCrypto As New DataSet
        Dim cryptoRows As IEnumerable(Of DataRow) = ds.Tables(0).AsEnumerable().Where(Function(row) row.Item("moneda") = "BTC")

        Dim tableCrypto As New DataTable
        If cryptoRows.Count > 0 Then
            tableCrypto = cryptoRows.CopyToDataTable()
        End If
        dsCrypto.Tables.Add(tableCrypto)

        Me.dgCryptoDeposits.DataSource = dsCrypto
        Me.dgCryptoDeposits.DataBind()

        Dim depositRows As IEnumerable(Of DataRow) = ds.Tables(0).AsEnumerable().Where(Function(row) row.Item("moneda") <> "BTC")
        Dim tableDeposits As New DataTable
        If depositRows.Count > 0 Then
            tableDeposits = depositRows.CopyToDataTable()
        End If
        ds = New DataSet()
        ds.Tables.Add(tableDeposits)

        Me.dgDepositos.DataSource = ds
        Me.dgDepositos.DataBind()
        'If Me.dgDepositos.Items.Count > 0 Then btnSave.Visible = True
        System.Threading.Thread.CurrentThread.CurrentCulture = ci

    End Sub

    Private Sub loadData()
        Try
            hplLogoCertificacion.Visible = False
            aInfo.Visible = False
            If MyBase.IsSupervisor Then
                aInfo.Visible = True
            End If
            aInfo.HRef = AppSettings("UrlInfoEmpresa") & "?idempresa=" & MyBase.cInfoActual.Empresa
            imgLogo.ImageUrl = AppSettings("VirtualDirectoryLogos") & "LogoCompany_" & Me.cInfoActual.Empresa & "?" & Now.ToString
            lblNombre.Text = Me.cInfoActual.HotelName
            lblAdress.Text = Me.cInfoActual.Address
            lblCity.Text = Me.cInfoActual.City & ", " & Me.cInfoActual.State
            Dim idAsociacion As Integer = Me.GetIdAsociation
            Dim ds As DataSet
            With New HotelSistema
                'verificamos si es usuario cadena
                If MyBase.isUserChain AndAlso Session("idCorporativoUserChain") <> "-1" Then
                    Dim idUsuario As Integer = -1
                    Dim idCorporativoUserChain As Integer = -1
                    Try
                        idUsuario = CType(Session("idUsuario"), Integer)
                        idCorporativoUserChain = CType(Session("idCorporativoUserChain"), Integer)
                    Catch ex As Exception
                        idUsuario = -1
                        idCorporativoUserChain = -1
                    End Try
                    ds = .GetHotelRecordatorios(Me.cInfoActual.Hotel, New Date(Now.Year, 1, 1), New Date(Now.Year, 12, 31), Now.Date, idUsuario, idCorporativoUserChain, idAsociacion)
                Else
                    ds = .GetHotelRecordatorios(Me.cInfoActual.Hotel, New Date(Now.Year, 1, 1), New Date(Now.Year, 12, 31), Now.Date, idAsociacion:=idAsociacion)
                End If

            End With
            If Not ds Is Nothing Then
                Dim ci As System.Globalization.CultureInfo
                ci = System.Threading.Thread.CurrentThread.CurrentCulture
                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)

                If MyBase.isUserChain AndAlso Session("idCorporativoUserChain") <> "-1" Then
                    Dim table As DataTable = ds.Tables(5)
                    Dim ind As Integer = 0
                    For Each dr As DataRow In ds.Tables(ReservaDatos.RESERVA_TABLE).Rows
                        Dim drnew As DataRow = table.NewRow()
                        For i As Integer = 0 To dr.ItemArray.Length - 1
                            drnew(i) = dr.ItemArray(i)
                        Next
                        table.Rows.InsertAt(drnew, 0 + ind)
                        ind += 1
                    Next
                    Me.dgReservations.DataSource = table
                Else
                    Me.dgReservations.DataSource = ds.Tables(ReservaDatos.RESERVA_TABLE)
                End If


                Me.dgReservations.DataBind()

                lblVerificar.Visible = False
                liVeririfar.Visible = False
                lblTarifas.Visible = True
                liTarifas.Visible = False
                lblConciliar.Visible = False
                liConciliar.Visible = False
                lblInprocess.Visible = False
                liInprocess.Visible = False

                lblFactura.Visible = False
                LiFactura.Visible = False




                If Not ds.Tables(ReservaDatos.RESERVA_TABLE) Is Nothing AndAlso ds.Tables(ReservaDatos.RESERVA_TABLE).Rows.Count > 0 Then
                    lblVerificar.Visible = True
                    liVeririfar.Visible = True
                End If
                If Not ds.Tables(FaresData.FARES_TABLE) Is Nothing AndAlso ds.Tables(FaresData.FARES_TABLE).Rows(0).Item(0) > 0 Then
                    lblTarifas.Visible = False
                    liTarifas.Visible = False
                End If
                If Not ds.Tables(HotelDatos.Table_ComisionDate) Is Nothing AndAlso ds.Tables(HotelDatos.Table_ComisionDate).Rows.Count > 0 Then
                    Dim lasconciliate As Date = ds.Tables(HotelDatos.Table_ComisionDate).Rows(0).Item(0)
                    If Now.Date >= lasconciliate Then
                        Me.lblConciliar.Text = PortalCulture.GetString("00506")

                    Else
                        'Me.lblConciliar.Text = String.Format(PortalCulture.GetString("00507"), DateDiff(DateInterval.Day, Now.Date, lasconciliate))
                        Me.lblConciliar.Text = String.Format(PortalCulture.GetString("00507"), lasconciliate.ToString("MMM/dd/yyyy"))
                    End If
                    lblConciliar.Visible = True
                    liConciliar.Visible = True
                End If
                'la consulta te devuelve los 2 últimos incluyendo el acceso actual
                Me.lblFechaLastAccess.Text = ""
                If ds.Tables(LogData.TABLE_LOG).Rows.Count > 1 Then
                    Me.lblFechaLastAccess.Text = CDate(ds.Tables(LogData.TABLE_LOG).Rows(1).Item(0)).ToString("MMM/dd/yyyy") & " " & CDate(ds.Tables(LogData.TABLE_LOG).Rows(1).Item(0)).ToString("hh:mm:ss tt")
                End If
                If ds.Tables(4).Rows.Count > 0 Then
                    Try
                        If CInt(ds.Tables(4).Rows(0).Item(0)) > 0 Then
                            lblInprocess.Text = String.Format(PortalCulture.GetString("00809"), ds.Tables(4).Rows(0).Item(0))
                            liInprocess.Visible = True
                            lblInprocess.Visible = True
                        End If
                    Catch ex As Exception
                    End Try
                End If
                System.Threading.Thread.CurrentThread.CurrentCulture = ci
            End If

            '
            'Try


            '    With New PaymentsModel
            '        Dim table As Oz.UniBilling.Hotels.DataAccess.PaymentModelDataSet.GetHotelPaymentRequirementByCompanyIDDataTable
            '        table = .SelectHotelPaymentRequirementByCompanyID(Me.cInfoActual.Empresa)
            '        If Not table Is Nothing AndAlso table.Rows.Count > 0 Then
            '            If table(0).PaymentRequirement = 3 Then
            '                lblHotelWithDebit.Visible = True
            '                lblHotelWithDebit.Text = PortalCulture.GetString("01338")
            '            End If

            '        End If

            '    End With
            'Catch ex As Exception

            'End Try

            Dim ds2 As DataSet
            With New informacionEmpresaFacade
                ds2 = .GetinformacionEmpresa(Me.cInfoActual.Empresa)
            End With

            If Not ds2 Is Nothing Then
                If cInfoActual.IdPais = "MX" Then
                    If Not ds2.Tables(InformacionEmpresaDatos.TABLE_factura) Is Nothing AndAlso ds2.Tables(InformacionEmpresaDatos.TABLE_factura).Rows.Count > 0 Then
                        LiFactura.Visible = True
                        lblFactura.Visible = True
                        lblFactura.Text = PortalCulture.GetString("00865") '"Hay facturas pendientes de pago"
                    End If
                End If

                'If Not (ds2.Tables(InformacionEmpresaDatos.TABLE_empresa)) Is Nothing AndAlso ds2.Tables(InformacionEmpresaDatos.TABLE_empresa).Columns.IndexOf("HotelActive") <> -1 Then
                '    If ds2.Tables(InformacionEmpresaDatos.TABLE_empresa).Rows.Count > 0 Then
                '        Try
                '            Dim hotelActive As Boolean = True
                '            hotelActive = CType(ds2.Tables(InformacionEmpresaDatos.TABLE_empresa).Rows(0)("HotelActive"), Boolean)
                '            If Not hotelActive Then
                '                LblHotelActive.Text = PortalCulture.GetString("01337")
                '                LblHotelActive.Visible = True
                '            End If
                '        Catch

                '        End Try
                '    End If

                'End If

                tblEstadoCuenta.Visible = False
                If Me.IsUsuarioNetRates Then

                End If
                If cInfoActual.IdPais = "MX" And ((Not Me.IsUsuarioNetRates) Or Me.IsSupervisor) Then
                    tblEstadoCuenta.Visible = True

                    lblSaldo_.Text = "-----"
                    lblTipoSaldo_.Text = "-----"
                    lblSinAplicar_.Text = "-----"

                    lblSinAplicar.Visible = False
                    lblSinAplicar_.Visible = False

                    If Not ds2.Tables(InformacionEmpresaDatos.TABLE_estadoCuenta) Is Nothing AndAlso ds2.Tables(InformacionEmpresaDatos.TABLE_estadoCuenta).Rows.Count > 0 Then
                        'Sin aplicar
                        If Not IsDBNull(ds2.Tables(InformacionEmpresaDatos.TABLE_estadoCuenta).Rows(0).Item(InformacionEmpresaDatos.FIELD_PaymentsNotAppliedMXN)) Then
                            Try
                                Dim pVery As Double = CType(ds2.Tables(InformacionEmpresaDatos.TABLE_estadoCuenta).Rows(0).Item(InformacionEmpresaDatos.FIELD_PaymentsVerifiedMXN), Double)
                                Dim pApli As Double = CType(ds2.Tables(InformacionEmpresaDatos.TABLE_estadoCuenta).Rows(0).Item(InformacionEmpresaDatos.FIELD_PaymentsAppliedMXN), Double)
                                pApli = (pVery - pApli)
                                If pApli > 0 Then
                                    lblSinAplicar_.Text = FCurrency(pApli, 2) & " MXN"
                                    lblSinAplicar.Visible = True
                                    lblSinAplicar_.Visible = True

                                End If
                            Catch

                            End Try

                        End If

                        'Saldo
                        If Not IsDBNull(ds2.Tables(InformacionEmpresaDatos.TABLE_estadoCuenta).Rows(0).Item(InformacionEmpresaDatos.FIELD_saldoMXN)) Then
                            lblSaldo_.Text = FCurrency(CType(ds2.Tables(InformacionEmpresaDatos.TABLE_estadoCuenta).Rows(0).Item(InformacionEmpresaDatos.FIELD_saldoMXN), Double), 2) & " MXN"

                            If Not IsDBNull(ds2.Tables(InformacionEmpresaDatos.TABLE_estadoCuenta).Rows(0).Item(InformacionEmpresaDatos.FIELD_tipoSaldoMXN)) Then
                                If (FCurrency(CType(ds2.Tables(InformacionEmpresaDatos.TABLE_estadoCuenta).Rows(0).Item(InformacionEmpresaDatos.FIELD_saldoMXN), Double), 2)) <> 0 Then
                                    If ds2.Tables(InformacionEmpresaDatos.TABLE_estadoCuenta).Rows(0).Item(InformacionEmpresaDatos.FIELD_tipoSaldoMXN) = "En contra" Then
                                        lblTipoSaldo_.Text = PortalCulture.GetString("00874")
                                    Else
                                        lblTipoSaldo_.Text = PortalCulture.GetString("00873")
                                    End If
                                End If
                            End If
                        End If

                    End If
                End If

                lblContrato.Text = String.Empty
                lblContrato.Visible = False
                liContrato.Visible = False
                If Not ds2.Tables(InformacionEmpresaDatos.TABLE_contrato) Is Nothing AndAlso ds2.Tables(InformacionEmpresaDatos.TABLE_contrato).Rows.Count > 0 Then
                    If (Not IsDBNull(ds2.Tables(InformacionEmpresaDatos.TABLE_contrato).Rows(0).Item(InformacionEmpresaDatos.FIELD_publicationDate))) And (Not IsDBNull(ds2.Tables(InformacionEmpresaDatos.TABLE_contrato).Rows(0).Item(InformacionEmpresaDatos.FIELD_durationContract))) Then
                        Dim fecha As New Date
                        fecha = ds2.Tables(InformacionEmpresaDatos.TABLE_contrato).Rows(0).Item(InformacionEmpresaDatos.FIELD_publicationDate)
                        fecha = fecha.AddMonths(ds2.Tables(InformacionEmpresaDatos.TABLE_contrato).Rows(0).Item(InformacionEmpresaDatos.FIELD_durationContract))

                        Dim ci As System.Globalization.CultureInfo
                        ci = System.Threading.Thread.CurrentThread.CurrentCulture
                        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
                        Select Case fecha.Date
                            Case Date.Now.Date
                                lblContrato.Text = PortalCulture.GetString("00870") & " (" & CDate(fecha).ToString("MMM/dd/yyyy") & ")"
                            Case Is < Date.Now.Date
                                lblContrato.Text = PortalCulture.GetString("00871", True) & " " & CDate(fecha).ToString("MMM/dd/yyyy")
                            Case Else
                                Dim fechaCaducar As Date = Date.Now
                                fechaCaducar = fechaCaducar.AddMonths(1)

                                If fecha.Date <= fechaCaducar.Date Then
                                    lblContrato.Text = PortalCulture.GetString("00872", True) & " " & CDate(fecha).ToString("MMM/dd/yyyy")
                                End If
                        End Select
                        System.Threading.Thread.CurrentThread.CurrentCulture = ci
                    End If
                End If

                If lblContrato.Text <> String.Empty Then
                    lblContrato.Visible = True
                    liContrato.Visible = True
                End If
            End If

            If cInfoActual.IdPais = "MX" Then
                Dim dsCuestionario As DataSet

                With New CuestionarioCertificacionFacade
                    dsCuestionario = .GetCuestionarioCertificacion(Me.cInfoActual.Empresa, Me.cInfoActual.Hotel)
                End With

                If Not dsCuestionario Is Nothing Then
                    If Not dsCuestionario.Tables(CuestionarioCertificacionData.TABLE_cuestionario) Is Nothing AndAlso dsCuestionario.Tables(CuestionarioCertificacionData.TABLE_cuestionario).Rows.Count > 0 Then
                        Try
                            Dim calificacion As Decimal = 0
                            Dim dsXML As DataSet = New DataSet
                            dsXML.ReadXml(AppSettings("rutaCuestionario"))

                            Dim dtCuestionarioHotel As New DataTable
                            Dim drCH As DataRow

                            Dim columnaIdPregunta As DataColumn = New DataColumn
                            columnaIdPregunta.DataType = System.Type.GetType("System.Int32")
                            columnaIdPregunta.ColumnName = "idPregunta"

                            dtCuestionarioHotel.Columns.Add(columnaIdPregunta)
                            dtCuestionarioHotel.Columns.Add("idHotel")
                            dtCuestionarioHotel.Columns.Add("idCuestionarioCertificacion")
                            dtCuestionarioHotel.Columns.Add("respuesta")
                            dtCuestionarioHotel.Columns.Add("fecha")

                            Dim keys(0) As DataColumn
                            keys(0) = columnaIdPregunta
                            dtCuestionarioHotel.PrimaryKey = keys

                            For cont As Integer = 0 To dsCuestionario.Tables(CuestionarioCertificacionData.TABLE_cuestionario).Rows.Count - 1
                                drCH = dtCuestionarioHotel.NewRow()
                                drCH("idPregunta") = dsCuestionario.Tables(CuestionarioCertificacionData.TABLE_cuestionario).Rows(cont).Item(CuestionarioCertificacionData.FIELD_idPregunta)
                                drCH("idHotel") = dsCuestionario.Tables(CuestionarioCertificacionData.TABLE_cuestionario).Rows(cont).Item(CuestionarioCertificacionData.FIELD_idHotel)
                                drCH("idCuestionarioCertificacion") = dsCuestionario.Tables(CuestionarioCertificacionData.TABLE_cuestionario).Rows(cont).Item(CuestionarioCertificacionData.FIELD_idCuestionarioCertificacion)
                                drCH("respuesta") = dsCuestionario.Tables(CuestionarioCertificacionData.TABLE_cuestionario).Rows(cont).Item(CuestionarioCertificacionData.FIELD_respuesta)
                                drCH("fecha") = dsCuestionario.Tables(CuestionarioCertificacionData.TABLE_cuestionario).Rows(cont).Item(CuestionarioCertificacionData.FIELD_fecha)
                                dtCuestionarioHotel.Rows.Add(drCH)
                            Next

                            Dim LabelNP As New Label
                            Dim ckbRespuestaActiva As New CheckBox
                            Dim numeroPreguntas As Integer
                            Dim preguntasActivas As Integer

                            If (dsXML.Tables.Count > 0) Then
                                Dim dr As DataRow
                                Dim dr2 As DataRow
                                For Each dr In dsXML.Tables(0).Rows
                                    LabelNP.Text = dr("orden")

                                    If LabelNP.Text Mod 1 <> 0 Then
                                        numeroPreguntas = numeroPreguntas + 1

                                        If (dtCuestionarioHotel.Rows.Count > 0) Then
                                            If Not dtCuestionarioHotel.Rows.Find(CType(dr("idPregunta"), Integer)) Is Nothing Then
                                                dr2 = dtCuestionarioHotel.Rows.Find(CType(dr("idPRegunta"), Integer))
                                                If Not dr2 Is Nothing Then
                                                    ckbRespuestaActiva.Checked = dr2("respuesta")
                                                    If ckbRespuestaActiva.Checked = True Then
                                                        preguntasActivas = preguntasActivas + 1
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                            End If

                            If numeroPreguntas > 0 Then
                                calificacion = Math.Round(preguntasActivas * 100 / numeroPreguntas * 100) / 100

                                If calificacion = 100 Then
                                    hplLogoCertificacion.Visible = True
                                End If
                            End If
                        Catch ex As Exception
                        End Try
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'If Not Me.IsPostBack Then
        '    SearchByDates()
        'End If
        Me.aInfo.InnerHtml = PortalCulture.GetString("00814")
        lblNew2.InnerHtml = PortalCulture.GetString("00801")
        lblNew3.InnerHtml = PortalCulture.GetString("00801")
        hplDeposits.Text = PortalCulture.GetString("00810")
        hplDeposits.NavigateUrl = GeRequestApplicationPath("/HotelAdministrator/Pages/Deposits.aspx")

        Me.hplResFrontDesk.Text = PortalCulture.GetString("00799")
        lblRecorda.Text = PortalCulture.GetString("00513")
        lblNoticias.Text = PortalCulture.GetString("00672")

        hplLIsting.Text = PortalCulture.GetString("00499")
        hplVerifyRes.Text = PortalCulture.GetString("00500")
        hplStatus.Text = PortalCulture.GetString("00501")
        hplInventory.Text = PortalCulture.GetString("00502")
        hplFares.Text = PortalCulture.GetString("00503")
        'hplListFares.Text = PortalCulture.GetString("00504")
        hplFacturacion.Text = PortalCulture.GetString("00505")

        hplNew1.Text = PortalCulture.GetString("00673")

        hplNew2.Text = PortalCulture.GetString("00802")

        hplNew3.Text = PortalCulture.GetString("00767")
        hplOthersNews.Text = PortalCulture.GetString("M0BT0000035")

        hplCertificacion.Text = PortalCulture.GetString("00875")
        lblNew4.InnerHtml = PortalCulture.GetString("00801")

        hplCertificacion.Visible = False
        lblNew4.Visible = False
        lblCertificacion.Visible = False
        If cInfoActual.IdPais = "MX" Then
            hplCertificacion.Visible = True
            lblNew4.Visible = True
            lblCertificacion.Visible = True
        End If

        Me.lblVerificar.Text = PortalCulture.GetString("00509")
        Me.lblTarifas.Text = PortalCulture.GetString("00508")
        lblLastResevations.Text = PortalCulture.GetString("00511")
        lblLastResevationsDep.Text = PortalCulture.GetString("01341")
        lblCryptoDeposits.Text = PortalCulture.GetString("01659")
        Me.lblLastAccess.Text = PortalCulture.GetString("00512", True)
        hplFrontDeskRes.Text = PortalCulture.GetString("00813")
        'el idioma del dg
        Me.dgReservations.Columns(dgcolumns.Cliente).HeaderText = PortalCulture.GetString("M000121")
        Me.dgReservations.Columns(dgcolumns.checkIn).HeaderText = PortalCulture.GetString("M000122")
        Me.dgReservations.Columns(dgcolumns.cantidad).HeaderText = PortalCulture.GetString("00062")
        Me.dgReservations.Columns(dgcolumns.NoReservacion).HeaderText = PortalCulture.GetString("M000119")

        hplNew2.Visible = False
        hplNew3.Visible = False
        If cInfoActual.IdPais = "MX" Then
            hplNew2.Visible = True
            hplNew3.Visible = True
        End If

        If IsUsuarioNivelHotel Then
            If Me.cInfoActual.UserPerfil = PaginaBase.PerfilHotel.Basico Then
                lblStatus.Visible = False
                hplStatus.Visible = False
            End If
        ElseIf Me.IsUsuarioHotel Then
            hplStatus.Visible = MyBase.PermissionSeePage(hplStatus.NavigateUrl)
            lblStatus.Visible = MyBase.PermissionSeePage(hplStatus.NavigateUrl)

            hplInventory.Visible = MyBase.PermissionSeePage(hplInventory.NavigateUrl)
            lblInventory.Visible = MyBase.PermissionSeePage(hplInventory.NavigateUrl)

            hplFares.Visible = MyBase.PermissionSeePage(hplFares.NavigateUrl)
            lblFares.Visible = MyBase.PermissionSeePage(hplFares.NavigateUrl)

            lblLIsting.Visible = MyBase.PermissionSeePage(hplLIsting.NavigateUrl)
            hplLIsting.Visible = MyBase.PermissionSeePage(hplLIsting.NavigateUrl)

            lblFacturacion.Visible = MyBase.PermissionSeePage(hplFacturacion.NavigateUrl)
            hplFacturacion.Visible = MyBase.PermissionSeePage(hplFacturacion.NavigateUrl)

            hplVerifyRes.Visible = MyBase.PermissionSeePage(hplVerifyRes.NavigateUrl)
            lblVerifyRes.Visible = MyBase.PermissionSeePage(hplVerifyRes.NavigateUrl)

            hplResFrontDesk.Visible = MyBase.PermissionSeePage(hplResFrontDesk.NavigateUrl)
            lblResFrontDesk.Visible = MyBase.PermissionSeePage(hplResFrontDesk.NavigateUrl)


            hplVerifyRes.Visible = MyBase.PermissionSeePage(hplVerifyRes.NavigateUrl)
            lblVerifyRes.Visible = MyBase.PermissionSeePage(hplVerifyRes.NavigateUrl)

            hplInventory.Visible = MyBase.PermissionSeePage(hplInventory.NavigateUrl)
            lblInventory.Visible = MyBase.PermissionSeePage(hplInventory.NavigateUrl)


            hplFares.Visible = MyBase.PermissionSeePage(hplFares.NavigateUrl)
            lblFares.Visible = MyBase.PermissionSeePage(hplFares.NavigateUrl)

            hplStatus.Visible = MyBase.PermissionSeePage(hplStatus.NavigateUrl)
            lblStatus.Visible = MyBase.PermissionSeePage(hplStatus.NavigateUrl)

            'end if
        End If

        Dim lk As LinkButton
        For Each i As DataGridItem In Me.dgReservations.Items
            If i.ItemType = ListItemType.AlternatingItem Or i.ItemType = ListItemType.Item Then
                lk = i.FindControl("lnkItinerario")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.ReservationsList, lk, "R")
                lk = i.FindControl("lnkVerificar")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.ConfirmReservations, lk, "R")
            End If
        Next

        lblEstadoCuenta.Text = PortalCulture.GetString("00856")
        lblSaldo.Text = PortalCulture.GetString("00859", True)
        lblTipoSaldo.Text = PortalCulture.GetString("00860", True)
        lblSinAplicar.Text = PortalCulture.GetString("01177", True)



        If Not MyBase.IsSupervisor AndAlso Not MyBase.isUserChain AndAlso MyBase.cInfoActual.Hotel <> 0 AndAlso MyBase.cInfoActual.EsMoroso Then
            dgReservations.Visible = False
        End If
    End Sub

    Private Sub dgReservations_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgReservations.ItemCommand
        If e.CommandName = "DetalleReserva" Then
            ' MyBase.redirectTo(PaginaBase.pages.ReservaDetailsV2, "?qs=" & e.Item.Cells(dgcolumns.ID).Text)
            MyBase.redirectTo(PaginaBase.pages.ReservationDetailsUI, "?qs=" & e.Item.Cells(dgcolumns.ID).Text)
        End If
    End Sub
    Private Sub loadHotels()

        If Usuario = 0 Then Exit Sub
        Dim reader As DataSet
        Dim idAsociacionHotel As Integer = Me.GetIdAsociation
        With New HotelSistema
            reader = .GetHotelsCompanyByUser(Usuario, idAsociacionHotel:=idAsociacionHotel)
        End With
        Dim cInfo As New companyInfo
        If Not reader.Tables(0) Is Nothing Then
            If reader.Tables(0).Rows.Count = 1 Then
                With reader.Tables(0).Rows(0)
                    cInfo = New companyInfo
                    cInfo.Rubro = .Item("idRubro")
                    cInfo.Hotel = .Item("idHotel")
                    cInfo.Empresa = .Item("idEmpresa")
                    cInfo.HotelName = .Item("Nombre")
                    cInfo.Address = .Item("Domicilio")
                    cInfo.City = .Item("Ciudad")
                    cInfo.State = .Item("Estado")
                    cInfo.Contact = .Item("Contacto_Nombre")
                    cInfo.Email = .Item("Contacto_Email")
                    cInfo.Phone = .Item("Telefono")
                    cInfo.EsMoroso = .Item("EsMoroso")
                    cInfo.UserPerfil = PaginaBase.PerfilHotel.Avanzado
                    cInfo.IdPais = .Item("idpais")
                    If Not .Item("UserPerfil") Is DBNull.Value Then
                        cInfo.UserPerfil = .Item("UserPerfil")
                    End If
                    ViewState.Item("cInfo_" & .Item("idHotel")) = cInfo
                    Me.cInfoActual = cInfo
                    Me.guardalog("/Portal/Pages/Welcome.aspx", PaginaBase.acciones.LogIn, "Han Accesado al sistema")
                End With
            Else
                MyBase.redirectTo(PaginaBase.pages.SearchHotel)
            End If
        End If

    End Sub
    Private Sub loadHotelUsuarioHotel()
        If Usuario = 0 Then Exit Sub
        Dim reader As SqlDataReader
        With New UsuarioHotelFacade
            reader = .LoadHotelsByUsuarioHotel(Usuario)
        End With
        While reader.Read
            Dim item As New ListItem(reader.Item("Nombre"), reader.Item("idHotel"))
            'Agregar al viewstate los datos del hotel
            Dim cInfo As New companyInfo
            cInfo.Rubro = reader.Item("idRubro")
            cInfo.Hotel = reader.Item("idHotel")
            cInfo.Empresa = reader.Item("idEmpresa")
            cInfo.HotelName = reader.Item("Nombre")
            cInfo.Address = reader.Item("Domicilio")
            cInfo.City = reader.Item("Ciudad")
            cInfo.State = reader.Item("Estado")
            cInfo.Contact = reader.Item("Contacto_Nombre")
            cInfo.Email = reader.Item("Contacto_Email")
            cInfo.Phone = reader.Item("Telefono")
            cInfo.IdPais = reader.Item("idpais")
            'cinfo.UserPerfil=reader.Item("")
            viewstate.Item("cInfo_" & reader.Item("idHotel")) = cInfo
            Me.cInfoActual = cInfo
        End While
        Dim DS As PermisosData
        With New PermisosFacade
            DS = .PermisosGetByUser(Usuario)
        End With
        For Each R As DataRow In DS.Tables(DS.PermisosTable).Rows
            Dim der As New DerechoUsuario
            der.Permiso = R(DS.PermisosField)
            der.PermisoName = R(DS.NameField)
            MyBase.PermisoUser(R(DS.NameField)) = der
        Next
        'aki hay que desabilitar los links
    End Sub

    Private Sub dgReservations_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReservations.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If dgReservations.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgReservations.CurrentPageIndex < dgReservations.PageCount - 1 Then
                Dim _next As New System.Web.UI.WebControls.LinkButton
                _next.CommandArgument = "Next"
                _next.CommandName = "Page"
                _next.Text = PortalCulture.GetString("00011") & "&nbsp;>"
                _next.CausesValidation = False

                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, _next)
            End If
        End If
    End Sub

    Private Sub dgReservations_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReservations.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Cliente).Text = PortalCulture.GetString("M000121")
            e.Item.Cells(dgcolumns.checkIn).Text = PortalCulture.GetString("M000122")
            e.Item.Cells(dgcolumns.cantidad).Text = PortalCulture.GetString("00062")
            e.Item.Cells(dgcolumns.NoReservacion).Text = PortalCulture.GetString("M000119")
            e.Item.Cells(dgcolumns.pmsACT).Text = PortalCulture.GetString("01051")
        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim permiso As Boolean = PermisionContentWelcome("welcome.aspx")

            Dim lk As LinkButton = e.Item.FindControl("lnkItinerario")
            If Not lk Is Nothing Then
                lk.Text = PortalCulture.GetString("00510")
            End If
            If Not permiso Then
                lk.Visible = False
            End If
            lk = e.Item.FindControl("lnkVerificar")
            If Not lk Is Nothing Then
                lk.Text = PortalCulture.GetString("M000654")
            End If
            If Not permiso Then
                lk.Visible = False
            End If

            e.Item.Cells(dgcolumns.checkIn).Text = CDate(e.Item.Cells(dgcolumns.checkIn).Text).ToString("MMM/dd/yyyy")

            If Not IsNumeric(e.Item.Cells(dgcolumns.NoReservacion).Text) Then
                e.Item.Cells(dgcolumns.NoReservacion).ToolTip = PortalCulture.GetString("00782")
            End If

            Dim movement As String = String.Empty
            movement = e.Item.Cells(dgcolumns.pmsACT).Text
            Select Case movement
                Case "SS"
                    e.Item.Cells(dgcolumns.pmsACT).Text = PortalCulture.GetString("01052")
                Case "CC"
                    e.Item.Cells(dgcolumns.pmsACT).Text = PortalCulture.GetString("01053")
                Case "XX"
                    e.Item.Cells(dgcolumns.pmsACT).Text = PortalCulture.GetString("01054")
            End Select
        End If
    End Sub

    Private Sub dgReservations_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgReservations.PageIndexChanged
        dgReservations.CurrentPageIndex = e.NewPageIndex
        dgReservations.SelectedIndex = -1
        loadData()
    End Sub

    Private Sub dgReservations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgReservations.SelectedIndexChanged
        Session("welcome_noReservacion") = String.Empty
        Session("welcome_noReservacion") = dgReservations.Items(dgReservations.SelectedIndex).Cells(dgcolumns.NoReservacionDato).Text
        Session("welcome_idhotel") = String.Empty
        If MyBase.isUserChain AndAlso Session("idCorporativoUserChain") <> "-1" Then
            Session("welcome_idhotel") = dgReservations.Items(dgReservations.SelectedIndex).Cells(dgcolumns.idHotel).Text
        End If
        MyBase.redirectTo(PaginaBase.pages.ConfirmReservas)
    End Sub

    Private Sub dgDepositos_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgDepositos.ItemCommand
        If e.CommandName = "DetalleReserva" Then
            'MyBase.redirectTo(PaginaBase.pages.ReservaDetailsV2, "?qs=" & e.Item.Cells(dgcolumns.ID).Text)
            MyBase.redirectTo(PaginaBase.pages.ReservationDetailsUI, "?qs=" & e.Item.Cells(dgcolumns.ID).Text)
        End If
    End Sub

    Private Sub dgCryptoDeposits_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCryptoDeposits.ItemCommand
        If e.CommandName = "DetalleReserva" Then
            'MyBase.redirectTo(PaginaBase.pages.ReservaDetailsV2, "?qs=" & e.Item.Cells(dgcolumns.ID).Text)
            MyBase.redirectTo(PaginaBase.pages.ReservationDetailsUI, "?qs=" & e.Item.Cells(dgcolumns.ID).Text)
        End If
    End Sub

    Private Sub dgDepositos_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDepositos.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If dgDepositos.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgDepositos.CurrentPageIndex < dgDepositos.PageCount - 1 Then
                Dim _next As New System.Web.UI.WebControls.LinkButton
                _next.CommandArgument = "Next"
                _next.CommandName = "Page"
                _next.Text = PortalCulture.GetString("00011") & "&nbsp;>"
                _next.CausesValidation = False

                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, _next)
            End If
        End If
    End Sub

    Private Sub dgCryptoDeposits_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCryptoDeposits.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If dgCryptoDeposits.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgCryptoDeposits.CurrentPageIndex < dgCryptoDeposits.PageCount - 1 Then
                Dim _next As New System.Web.UI.WebControls.LinkButton
                _next.CommandArgument = "Next"
                _next.CommandName = "Page"
                _next.Text = PortalCulture.GetString("00011") & "&nbsp;>"
                _next.CausesValidation = False

                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, _next)
            End If
        End If
    End Sub

    Private Sub dgDepositos_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDepositos.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Cliente).Text = PortalCulture.GetString("M000121")
            e.Item.Cells(dgcolumns.checkIn).Text = PortalCulture.GetString("M000122")
            e.Item.Cells(dgcolumns.cantidad).Text = PortalCulture.GetString("00062")
            e.Item.Cells(dgcolumns.NoReservacion).Text = PortalCulture.GetString("M000119")
        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim permiso As Boolean = PermisionContentWelcome("welcome.aspx")
            Dim lk As LinkButton = e.Item.FindControl("lnkItinerario")
            If Not lk Is Nothing Then
                lk.Text = PortalCulture.GetString("00510")
            End If
            If Not permiso Then
                lk.Visible = False
            End If
            lk = e.Item.FindControl("lnkVerificar")
            If Not lk Is Nothing Then
                lk.Text = PortalCulture.GetString("01342")
            End If
            If Not permiso Then
                lk.Visible = False
            End If


            If (e.Item.Cells(dgcolumns.IsNetRateUv).Text = "True") Or (e.Item.Cells(dgcolumns.deposittarget).Text = "UV") Then
                If Not (MyBase.IsSupervisor Or (MyBase.IsUsuarioHotelAssociation And MyBase.IdAsociation = 1)) Then
                    lk.Visible = False
                End If
            End If

            Dim add As Integer
            add = 1
            If CDate(e.Item.Cells(dgcolumns.checkIn).Text).DayOfWeek = DayOfWeek.Saturday Then add = 2
            If CDate(e.Item.Cells(dgcolumns.checkIn).Text).DayOfWeek = DayOfWeek.Friday Then add = 3
            Dim fechar As Date
            fechar = DateAdd("d", add, CDate(CDate(e.Item.Cells(dgcolumns.checkIn).Text).ToString("yyyy/MM/dd")))
            If fechar < CDate(Now.ToString("yyyy/MM/dd")) Then
                'e.Item.BackColor = Color.FromName("#F88158") 'dgReservas.BackColor.LightPink
                e.Item.ForeColor = Color.FromKnownColor(KnownColor.Red)
            Else
                'ck.Visible = False
            End If
            e.Item.Cells(dgcolumns.checkIn).Text = CDate(e.Item.Cells(dgcolumns.checkIn).Text).ToString("MMM/dd/yyyy")

            If Not IsNumeric(e.Item.Cells(dgcolumns.NoReservacion).Text) Then
                e.Item.Cells(dgcolumns.NoReservacion).ToolTip = PortalCulture.GetString("01343")
            End If
        End If
    End Sub

    Private Sub dgCryptoDeposits_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCryptoDeposits.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Cliente).Text = PortalCulture.GetString("M000121")
            e.Item.Cells(dgcolumns.checkIn).Text = PortalCulture.GetString("M000122")
            e.Item.Cells(dgcolumns.cantidad).Text = PortalCulture.GetString("00062")
            e.Item.Cells(dgcolumns.NoReservacion).Text = PortalCulture.GetString("M000119")
        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim permiso As Boolean = PermisionContentWelcome("welcome.aspx")
            Dim lk As LinkButton = e.Item.FindControl("lnkItinerario")
            If Not lk Is Nothing Then
                lk.Text = PortalCulture.GetString("00510")
            End If
            If Not permiso Then
                lk.Visible = False
            End If
            lk = e.Item.FindControl("lnkVerificar")
            If Not lk Is Nothing Then
                lk.Text = PortalCulture.GetString("01342")
            End If
            If Not permiso Then
                lk.Visible = False
            End If


            If (e.Item.Cells(dgcolumns.IsNetRateUv).Text = "True") Or (e.Item.Cells(dgcolumns.deposittarget).Text = "UV") Then
                If Not (MyBase.IsSupervisor Or (MyBase.IsUsuarioHotelAssociation And MyBase.IdAsociation = 1)) Then
                    lk.Visible = False
                End If
            End If

            Dim add As Integer
            add = 1
            If CDate(e.Item.Cells(dgcolumns.checkIn).Text).DayOfWeek = DayOfWeek.Saturday Then add = 2
            If CDate(e.Item.Cells(dgcolumns.checkIn).Text).DayOfWeek = DayOfWeek.Friday Then add = 3
            Dim fechar As Date
            fechar = DateAdd("d", add, CDate(CDate(e.Item.Cells(dgcolumns.checkIn).Text).ToString("yyyy/MM/dd")))
            If fechar < CDate(Now.ToString("yyyy/MM/dd")) Then
                'e.Item.BackColor = Color.FromName("#F88158") 'dgReservas.BackColor.LightPink
                e.Item.ForeColor = Color.FromKnownColor(KnownColor.Red)
            Else
                'ck.Visible = False
            End If
            e.Item.Cells(dgcolumns.checkIn).Text = CDate(e.Item.Cells(dgcolumns.checkIn).Text).ToString("MMM/dd/yyyy")

            If Not IsNumeric(e.Item.Cells(dgcolumns.NoReservacion).Text) Then
                e.Item.Cells(dgcolumns.NoReservacion).ToolTip = PortalCulture.GetString("01343")
            End If
        End If
    End Sub

    Private Sub dgDepositos_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDepositos.PageIndexChanged
        dgDepositos.CurrentPageIndex = e.NewPageIndex
        dgDepositos.SelectedIndex = -1
        SearchByDates()
    End Sub

    Private Sub dgCryptoDeposits_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgCryptoDeposits.PageIndexChanged
        dgCryptoDeposits.CurrentPageIndex = e.NewPageIndex
        dgCryptoDeposits.SelectedIndex = -1
        SearchByDates()
    End Sub

    Private Sub dgDepositos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgDepositos.SelectedIndexChanged

        Session("welcome_noReservacion") = String.Empty
        'dgDepositos.CurrentPageIndex
        Session("welcome_noReservacion") = dgDepositos.Items(dgDepositos.SelectedIndex).Cells(dgcolumns.NoReservacionDato).Text
        Session("welcome_idhotel") = String.Empty
        If MyBase.isUserChain AndAlso Session("idCorporativoUserChain") <> "-1" Then
            'Session("welcome_idhotel") = dgReservations.Items(dgDepositos.SelectedIndex).Cells(dgcolumns.idHotel).Text
        End If
        MyBase.redirectTo(PaginaBase.pages.Deposito)
    End Sub

    Private Sub dgCryptoDeposits_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgCryptoDeposits.SelectedIndexChanged

        Session("welcome_noReservacion") = String.Empty
        'dgDepositos.CurrentPageIndex
        Session("welcome_noReservacion") = dgCryptoDeposits.Items(dgCryptoDeposits.SelectedIndex).Cells(dgcolumns.NoReservacionDato).Text
        Session("welcome_idhotel") = String.Empty
        If MyBase.isUserChain AndAlso Session("idCorporativoUserChain") <> "-1" Then
            'Session("welcome_idhotel") = dgReservations.Items(dgDepositos.SelectedIndex).Cells(dgcolumns.idHotel).Text
        End If
        MyBase.redirectTo(PaginaBase.pages.Deposito)
    End Sub
End Class
