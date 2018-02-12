Imports Portal.Hotel

Partial Class ConventionsMeetings
    Inherits PaginaBase

#Region "Enums"
    Enum columnas
        idConvenciones = 0
        ContactoNombre = 1
        ContactoEmail = 2
        Llegada = 3
        Salida = 4
        Estatus = 5
        EstatusText = 6
        Operacion = 7
    End Enum
#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents btnNuevo As System.Web.UI.WebControls.Button
    Protected WithEvents btnGuardar As System.Web.UI.WebControls.Button
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblDeleteError As System.Web.UI.WebControls.Label
    Protected WithEvents PanelRooms As System.Web.UI.WebControls.Panel
    Protected WithEvents UpdateImgs As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Event of Page"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            LoadDataSource()
            TablaPincipal.Visible = False
            LoadData()
        End If
        Me.ResizefrmPrincipal()
    End Sub


    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        btnLoadStatus.Text = PortalCulture.GetString("00149") 'LOAD
        lblTitleForm.Text = PortalCulture.GetString("01008") 'Convenciones y Reuniones
        lbl_NombreEmpresa.Text = PortalCulture.GetString("00922") 'Empresa y/o Grupo
        lbl_Noches.Text = PortalCulture.GetString("00923") 'Número de Noches que durará su evento
        lbl_TipoEvento.Text = PortalCulture.GetString("00924") '¿Cual es el tipo de evento que va a realizar?
        lbl_Fechas.Text = PortalCulture.GetString("M000441") 'Fechas
        lbl_Llegada.Text = PortalCulture.GetString("M0BT0000043") 'Llegada
        lbl_Salida.Text = PortalCulture.GetString("M0BT0000044") 'Salida
        lbl_Flexibilidad.Text = PortalCulture.GetString("00925") '¿Su evento y/o grupo tiene flexibilidad para cambiar de fechas? 
        lbl_FlexibilidadFechas.Text = PortalCulture.GetString("00926") '¿Que otras fechas? 
        lbl_FlexibilidadLlegada.Text = PortalCulture.GetString("M0BT0000043") 'Llegada
        lbl_FlexibilidadSalida.Text = PortalCulture.GetString("M0BT0000044") 'Salida
        lbl_NumeroPersonas.Text = PortalCulture.GetString("00927") 'Número de personas
        lbl_NumeroHabitaciones.Text = PortalCulture.GetString("00928") 'Número Total de Habitaciones solicitadas
        lbl_HabitacionesSon.Text = PortalCulture.GetString("00929") 'De las cuales son
        lbl_NumeroHabitacionesSencillas.Text = PortalCulture.GetString("00194") 'Sencilla
        lbl_NumeroHabitacionesDobles.Text = PortalCulture.GetString("00195") 'Dobles
        lbl_NumeroHabitacionesTriples.Text = PortalCulture.GetString("M000396") 'Triple
        lbl_NumeroHabitacionesJrSuites.Text = PortalCulture.GetString("00930") 'Jr.Suites
        lbl_NumeroHabitacionesMasterSuites.Text = PortalCulture.GetString("00931") 'Master Suites
        lbl_NumeroHabitacionesSuitePresidente.Text = PortalCulture.GetString("00932") 'Suite Presidencial
        lbl_Hoteles.Text = PortalCulture.GetString("00933") '¿Que hotel(es) en especial de Hoteles Misión?
        lbl_Salones.Text = PortalCulture.GetString("00934") '¿Requiere Salones para Sesionar?
        lbl_Salon.Text = PortalCulture.GetString("00935") 'Salones
        lbl_Salon1.Text = PortalCulture.GetString("00936") + " 1" 'Salones
        lbl_Salon2.Text = PortalCulture.GetString("00936") + " 2" 'Salones
        lbl_Salon3.Text = PortalCulture.GetString("00936") + " 3" 'Salones
        lbl_TipoMontaje.Text = PortalCulture.GetString("00937") 'Tipo Montaje
        lbl_NumeroSalonPersona.Text = PortalCulture.GetString("00927") 'Número de personas
        lbl_AudioVisual.Text = PortalCulture.GetString("00938") '¿Requiere Equipo Audiovisual?
        lbl_AudioVisualPantalla.Text = PortalCulture.GetString("00939") 'pantalla
        lbl_AudioVisualEquipoSonido.Text = PortalCulture.GetString("00940") 'Equipo de Sonido con micrófonos
        lbl_AudioVisualAcetatos.Text = PortalCulture.GetString("00941") 'Proyector de acetatos
        lbl_AudioVisualProyectorTransparencias.Text = PortalCulture.GetString("00942") 'Proyector de transparencias
        lbl_AudioVisualCanion.Text = PortalCulture.GetString("00943") 'cañon
        lbl_AudioVisualLapTop.Text = PortalCulture.GetString("00944") 'laptop
        lbl_AudioVisualCopiadora.Text = PortalCulture.GetString("00945") 'Copiadora
        lbl_AudioVisualOtro.Text = PortalCulture.GetString("00946") 'Otro
        lbl_Alimentos.Text = PortalCulture.GetString("00948") 'Número de Alimentos
        lbl_AlimentosDesayunos.Text = PortalCulture.GetString("00949") 'Desayunos
        lbl_AlimentosComidas.Text = PortalCulture.GetString("00950") 'Comidas
        lbl_AlimentosCenas.Text = PortalCulture.GetString("00951") 'Cenas
        lbl_AlimentosRequiere.Text = PortalCulture.GetString("00952") 'Pos alimentos los requiere
        lbl_Bebidas.Text = PortalCulture.GetString("00953") 'Sus bebidas las requiere por
        lblBebidasRefresco.Text = PortalCulture.GetString("00954") 'Refrescos
        lblBebidasBotella.Text = PortalCulture.GetString("00955") 'Botella
        lblBebidasBarraImportada.Text = PortalCulture.GetString("00956") 'Barra Importada
        lblBebidasDescorcheBotella.Text = PortalCulture.GetString("00957") 'Descorche por Botella
        lblBebidasDescorchePersona.Text = PortalCulture.GetString("00958") 'Descorche por Persona
        lblBebidasCervezas.Text = PortalCulture.GetString("00959") 'Cervezas
        lblBebidasCopeo.Text = PortalCulture.GetString("00960") 'Copeo
        lblBebidasAguasFrutas.Text = PortalCulture.GetString("00961") 'Aguas de Frutas
        lblBebidasBarraNacional.Text = PortalCulture.GetString("00962") 'Barra Nacional
        lbl_BebidasEN.Text = PortalCulture.GetString("00963") 'Las bebidas alcohólicas solo las desea en
        lblBebidasEnCenas.Text = PortalCulture.GetString("00965") 'Cenas
        lblBebidasEnComidas.Text = PortalCulture.GetString("00964") 'Comidas
        lblBebidasEnOtro.Text = PortalCulture.GetString("00946") 'Otros
        lbl_NochesTema.Text = PortalCulture.GetString("00966") '¿Requiere de Noches Tema?
        lblNochesTemaPalenque.Text = PortalCulture.GetString("00967") 'palenque
        lblNochesTemaCasino.Text = PortalCulture.GetString("00968") 'Casino
        lblNochesTemaOtras.Text = PortalCulture.GetString("00969") 'Otros
        lbl_NochesTemaOtrasCuales.Text = PortalCulture.GetString("00985") 'Cuales
        lbl_ServiciosExternos.Text = PortalCulture.GetString("00970") 'Requiere de Servicios Externos
        lblServiciosExternosPeleaGallos.Text = PortalCulture.GetString("00971") 'Pelea de Gallos
        lblServiciosExternosMariachi.Text = PortalCulture.GetString("00972") 'Mariachi
        lblServiciosExternosTrio.Text = PortalCulture.GetString("00973") 'Trio
        lblServiciosExternosCarpas.Text = PortalCulture.GetString("00974") 'Carpas
        lblServiciosExternosManteleriaEspecial.Text = PortalCulture.GetString("00975") 'Mantelería Especial
        lblServiciosExternosCentroMesa.Text = PortalCulture.GetString("00976") 'Centros de Mesa
        lblServiciosExternosEquipoAudioVisual.Text = PortalCulture.GetString("00977") 'Equipo Audiovisual Especial
        lblServiciosExternosRecorridoPorCiudad.Text = PortalCulture.GetString("00978") 'Recorrido por Ciudad
        lbl_EventoPorAgenciaViajes.Text = PortalCulture.GetString("00979") 'Su evento será manejado por una agencia de Viajes
        lblEventoPorAgenciaViajesSi.Text = PortalCulture.GetString("00051") 'Si
        lblEventoPorAgenciaViajesNo.Text = PortalCulture.GetString("00052") 'No
        lbl_CotizadoEn.Text = PortalCulture.GetString("00980") 'Su presupuesto lo requiere cotizado en
        lblCotizadoEnPaquete.Text = PortalCulture.GetString("00981") 'Paquete
        lblCotizadoEnHabitacionesAlimentos.Text = PortalCulture.GetString("00982") 'Habitaciones y Alimentos por separado
        lblCotizadoEnOtro.Text = PortalCulture.GetString("00969") 'Otros

        lbl_RequerimiestosEspeciales.Text = PortalCulture.GetString("00983") 'Requerimientos Especiales
        lbl_Contacto.Text = PortalCulture.GetString("00984") 'Datos para poder contactarlo
        lbl_ContactoNombre.Text = PortalCulture.GetString("00073")  'Nombre
        lbl_ContactoEmpresa.Text = PortalCulture.GetString("M000614")  'Empresa
        lbl_ContactoCalle.Text = PortalCulture.GetString("00986")  'Calle
        lbl_ContactoTelefono.Text = PortalCulture.GetString("00164") 'Telefono
        lbl_ContactoFax.Text = PortalCulture.GetString("00837")  'Fax
        lbl_ContactoPuesto.Text = PortalCulture.GetString("00987") 'Puesto
        lbl_ContactoCiudad.Text = PortalCulture.GetString("00254") 'Ciudad
        lbl_ContactoColonia.Text = PortalCulture.GetString("00988") 'Colonia
        lbl_ContactoCodigoPostal.Text = PortalCulture.GetString("M0BT0000164") 'Codigo postal
        lbl_ContactoEmail.Text = PortalCulture.GetString("00726") 'Correo electrónico

        lblServiciosExternosOtros.Text = PortalCulture.GetString("00969") 'Otros


    End Sub

#End Region

#Region "Private Metod and Functions"
    Private Sub LoadTypeStutus(ByRef dll As DropDownList, ByVal isEdit As Boolean)

        If Not dll Is Nothing Then

            dll.Items.Clear()
            If Not isEdit Then
                dll.Items.Add(New ListItem(PortalCulture.GetString("M000058"), "0"))
            End If

            dll.Items.Add(New ListItem(PortalCulture.GetString("00439"), "1"))
            dll.Items.Add(New ListItem(PortalCulture.GetString("00151"), "2"))
            dll.Items.Add(New ListItem(PortalCulture.GetString("M000059"), "3"))
            If Not isEdit Then
                dll.Items.Add(New ListItem(PortalCulture.GetString("M000258"), "-1"))

            End If
        End If


    End Sub

    Private Sub LoadDataSource()
        Dim manager As ConventionsMeetingsFacade = New ConventionsMeetingsFacade
        DataSource = manager.GetConventionsMeetingsDataAccessByIdHotel(cInfoActual.Hotel)
    End Sub
    Private Sub LoadData()
        LoadTypeStutus(ddlEstatus, False)
        LoadGrid()

    End Sub

    Private Sub LoadGrid()
        grid.DataSource = Nothing
        grid.SelectedIndex = -1

        If Not DataSource Is Nothing Then
            Dim data As ConventionsMeetingsData = CType(DataSource, ConventionsMeetingsData)
            Dim dv As DataView = New DataView(data.Tables(data.TABLE_CONVENCIONES))
            Select Case ddlEstatus.SelectedItem.Value
                Case "0"
                    dv.RowFilter = "Estatus = 0"
                Case "1"
                    dv.RowFilter = "Estatus = 1"
                Case "2"
                    dv.RowFilter = "Estatus = 2"
                Case "3"
                    dv.RowFilter = "Estatus = 3"
            End Select
            grid.DataSource = dv

            Dim pi As Integer = grid.CurrentPageIndex
            Dim np As Integer = 0
            Dim pz As Integer = grid.PageSize
            If (pz > 0) Then
                np = dv.Count / pz
                If (dv.Count Mod pz > 0) Then
                    np += 1
                End If
            End If
            If (pi >= np) Then
                grid.CurrentPageIndex = IIf((np - 1) >= 0, (np - 1), 0)
            End If
        Else
            grid.CurrentPageIndex = 0
        End If
        grid.DataBind()

    End Sub

    Private Function GetTypeSalon(ByVal id As Integer) As String
        Select Case id

            Case 1
                Return PortalCulture.GetString("00999") ' "Auditorio"
            Case 2
                Return PortalCulture.GetString("01000") '"Banquete"
            Case 3
                Return PortalCulture.GetString("01001") '"Escuela"
            Case 4
                Return PortalCulture.GetString("01002") '"Herradura"
            Case 5
                Return PortalCulture.GetString("01003") '"Cocktail"
            Case 6
                Return PortalCulture.GetString("01004") '"Cena-Baile"
            Case 7
                Return PortalCulture.GetString("01005") '"Ruso"
            Case 8
                Return PortalCulture.GetString("01006") '"Tipo"
            Case 9
                Return PortalCulture.GetString("01007") '"Imperial"
            Case 10
                Return PortalCulture.GetString("M0BT0000335") '"Otro"
        End Select
        Return ""

    End Function

    Private Function GetTypeEvent(ByVal id As Integer) As String


        Select Case id

            Case 1
                Return PortalCulture.GetString("00989") '"Congreso"
            Case 2
                Return PortalCulture.GetString("00990") '"Convención"
            Case 3
                Return PortalCulture.GetString("00991") '"Incentivo"
            Case 4
                Return PortalCulture.GetString("00992") '"Capacitación"
            Case 5
                Return PortalCulture.GetString("00993") '"Sesión"
            Case 6
                Return PortalCulture.GetString("00994") '"Seminario"
            Case 7
                Return PortalCulture.GetString("00995") '"Lanzamiento"
            Case 8
                Return PortalCulture.GetString("00996") '"Exposición"
            Case 9
                Return PortalCulture.GetString("M0BT0000335") '"Otro"
        End Select
        Return ""

    End Function

    Private Sub ClearDataConventions()
        TablaOtraFechas.Visible = False
        lblNombreEmpresa.Text = ""
        lblNoches.Text = ""
        lblTipoEvento.Text = ""
        lblLlegada.Text = ""
        lblSalida.Text = ""
        lblFlexibilidad.Text = ""
        lblFlexibilidadLlegada.Text = ""
        lblFlexibilidadSalida.Text = ""
        lblNumeroPersonas.Text = ""
        lblNumeroHabitaciones.Text = ""
        lblNumeroHabitacionesSencillas.Text = ""
        lblNumeroHabitacionesDobles.Text = ""
        lblNumeroHabitacionesTriples.Text = ""
        lblNumeroHabitacionesJrSuites.Text = ""
        lblNumeroHabitacionesMasterSuites.Text = ""
        lblNumeroHabitacionesSuitePresidente.Text = ""
        lblHoteles.Text = ""
        lblTipoMontaje1.Text = ""
        lblTipoMontaje2.Text = ""
        lblTipoMontaje3.Text = ""
        lblNumeroSalonPersona1.Text = ""
        lblNumeroSalonPersona2.Text = ""
        lblNumeroSalonPersona3.Text = ""
        lblAudioVisualPantalla.Text = ""
        lblAudioVisualEquipoSonido.Text = ""
        lblAudioVisualAcetatos.Text = ""
        lblAudioVisualProyectorTransparencias.Text = ""
        lblAudioVisualCanion.Text = ""
        lblAudioVisualLapTop.Text = ""
        lblAudioVisualCopiadora.Text = ""
        lblAudioVisualOtro.Text = ""
        lblAudioVisualOtro.Text = ""
        lblAlimentosDesayunos.Text = ""
        lblAlimentosComidas.Text = ""
        lblAlimentosCenas.Text = ""
        lblAlimentosRequiere.Text = ""

        chkBebidasRefresco.Checked = False
        chkBebidasBotella.Checked = False
        chkBebidasBarraImportada.Checked = False
        chkBebidasDescorcheBotella.Checked = False
        chkBebidasDescorchePersona.Checked = False
        chkBebidasCervezas.Checked = False
        chkBebidasCopeo.Checked = False
        chkBebidasAguasFrutas.Checked = False
        chkBebidasBarraNacional.Checked = False


        chkBebidasEnComidas.Checked = False
        chkBebidasEnCenas.Checked = False
        chkBebidasEnOtro.Checked = False

        chkNochesTemaPalenque.Checked = False
        chkNochesTemaCasino.Checked = False
        chkNochesTemaOtras.Checked = False

        lblNochesTemaOtrasCuales.Text = ""

        chkServiciosExternosPeleaGallos.Checked = False
        chkServiciosExternosMariachi.Checked = False
        chkServiciosExternosTrio.Checked = False
        chkServiciosExternosCarpas.Checked = False
        chkServiciosExternosManteleriaEspecial.Checked = False
        chkServiciosExternosCentroMesa.Checked = False
        chkServiciosExternosEquipoAudioVisual.Checked = False
        chkServiciosExternosRecorridoPorCiudad.Checked = False
        chkServiciosExternosOtros.Checked = False


        rbEventoPorAgenciaViajesSi.Checked = True
        rbCotizadoEnPaquete.Checked = True


        lblRequerimiestosEspeciales.Text = ""


        lblContactoNombre.Text = ""
        lblContactoEmpresa.Text = ""
        lblContactoCalle.Text = ""
        lblContactoTelefono.Text = ""
        lblContactoFax.Text = ""
        lblContactoPuesto.Text = ""
        lblContactoCiudad.Text = ""
        lblContactoColonia.Text = ""
        lblContactoCodigoPostal.Text = ""
        lblContactoEmail.Text = ""
    End Sub

    Private Sub SelectData(ByVal id As Integer)
        TablaPincipal.Visible = False
        Try
            If Not DataSource Is Nothing Then

                Dim data As ConventionsMeetingsData = CType(DataSource, ConventionsMeetingsData)
                Dim drs As DataRow() = data.Tables(data.TABLE_CONVENCIONES).Select(String.Format("idConvenciones = {0}", id))
                If drs.Length > 0 Then
                    TablaPincipal.Visible = True
                    Dim dr As DataRow = drs(0)
                    TablaPincipal.Visible = True
                    ClearDataConventions()

                    If Not dr.IsNull(data.FIELD_NombreEmpresa) Then
                        lblNombreEmpresa.Text = dr(data.FIELD_NombreEmpresa)
                    End If

                    If Not dr.IsNull(data.FIELD_Noches) Then
                        lblNoches.Text = dr(data.FIELD_Noches)
                    End If
                    If Not dr.IsNull(data.FIELD_TipoEvento) Then
                        lblTipoEvento.Text = GetTypeEvent(dr(data.FIELD_TipoEvento))
                        If dr(data.FIELD_TipoEvento) = 8 Then
     
                        End If
                    End If

                    If Not dr.IsNull(data.FIELD_Llegada) Then
                        lblLlegada.Text = CType(dr(data.FIELD_Llegada), DateTime).ToString("dd-MMM-yyyy")
                    End If
                    If Not dr.IsNull(data.FIELD_Salida) Then
                        lblSalida.Text = CType(dr(data.FIELD_Salida), DateTime).ToString("dd-MMM-yyyy")
                    End If
                    If Not dr.IsNull(data.FIELD_FlexibilidadFechas) Then
                        If dr(data.FIELD_FlexibilidadFechas) Then
                            TablaOtraFechas.Visible = True
                            lblFlexibilidad.Text = "Si"
                        Else
                            lblFlexibilidad.Text = "No"
                        End If


                    End If
                    If Not dr.IsNull(data.FIELD_FlexibilidadLlegada) Then
                        lblFlexibilidadLlegada.Text = CType(dr(data.FIELD_FlexibilidadLlegada), DateTime).ToString("dd-MMM-yyyy")
                    End If
                    If Not dr.IsNull(data.FIELD_FexibilidadSalida) Then
                        lblFlexibilidadSalida.Text = CType(dr(data.FIELD_FexibilidadSalida), DateTime).ToString("dd-MMM-yyyy")
                    End If
                    If Not dr.IsNull(data.FIELD_NumeroPersonas) Then
                        lblNumeroPersonas.Text = dr(data.FIELD_NumeroPersonas)
                    End If
                    If Not dr.IsNull(data.FIELD_NumeroHabitaciones) Then
                        lblNumeroHabitaciones.Text = dr(data.FIELD_NumeroHabitaciones)
                    End If
                    If Not dr.IsNull(data.FIELD_NumeroHabitacionesSencillas) Then
                        lblNumeroHabitacionesSencillas.Text = dr(data.FIELD_NumeroHabitacionesSencillas)
                    End If
                    If Not dr.IsNull(data.FIELD_NumeroHabitacionesDobles) Then
                        lblNumeroHabitacionesDobles.Text = dr(data.FIELD_NumeroHabitacionesDobles)
                    End If
                    If Not dr.IsNull(data.FIELD_NumeroHabitacionesTriples) Then
                        lblNumeroHabitacionesTriples.Text = dr(data.FIELD_NumeroHabitacionesTriples)
                    End If
                    If Not dr.IsNull(data.FIELD_NumeroHabitacionesJrSuites) Then
                        lblNumeroHabitacionesJrSuites.Text = dr(data.FIELD_NumeroHabitacionesJrSuites)
                    End If
                    If Not dr.IsNull(data.FIELD_NumeroHabitacionesMasterSuites) Then
                        lblNumeroHabitacionesMasterSuites.Text = dr(data.FIELD_NumeroHabitacionesMasterSuites)
                    End If
                    If Not dr.IsNull(data.FIELD_NumeroHabitacionesSiutePresidencial) Then
                        lblNumeroHabitacionesSuitePresidente.Text = dr(data.FIELD_NumeroHabitacionesSiutePresidencial)
                    End If

                    If Not dr.IsNull(data.FIELD_Salon1Tipo) Then
                        lblTipoMontaje1.Text = GetTypeSalon(dr(data.FIELD_Salon1Tipo))
                    End If

                    If Not dr.IsNull(data.FIELD_Salon2Tipo) Then
                        lblTipoMontaje2.Text = GetTypeSalon(dr(data.FIELD_Salon2Tipo))
                    End If

                    If Not dr.IsNull(data.FIELD_Salon3Tipo) Then
                        lblTipoMontaje3.Text = GetTypeSalon(dr(data.FIELD_Salon3Tipo))
                    End If

                    If Not dr.IsNull(data.FIELD_Salon1Personas) Then
                        lblNumeroSalonPersona1.Text = dr(data.FIELD_Salon1Personas)
                    End If

                    If Not dr.IsNull(data.FIELD_Salon2Personas) Then
                        lblNumeroSalonPersona2.Text = dr(data.FIELD_Salon2Personas)
                    End If

                    If Not dr.IsNull(data.FIELD_Salon3Personas) Then
                        lblNumeroSalonPersona3.Text = dr(data.FIELD_Salon3Personas)
                    End If

                    If Not dr.IsNull(data.FIELD_AudioVisualPantalla) Then
                        lblAudioVisualPantalla.Text = dr(data.FIELD_AudioVisualPantalla)
                    End If

                    If Not dr.IsNull(data.FIELD_AudioVisualEquipoSonido) Then
                        lblAudioVisualEquipoSonido.Text = dr(data.FIELD_AudioVisualEquipoSonido)
                    End If

                    If Not dr.IsNull(data.FIELD_AudioVisualProyectorAcetatos) Then
                        lblAudioVisualAcetatos.Text = dr(data.FIELD_AudioVisualProyectorAcetatos)
                    End If

                    If Not dr.IsNull(data.FIELD_AudioVisualProyectorTransparencias) Then
                        lblAudioVisualProyectorTransparencias.Text = dr(data.FIELD_AudioVisualProyectorTransparencias)
                    End If

                    If Not dr.IsNull(data.FIELD_AudioVisualCanion) Then
                        lblAudioVisualCanion.Text = dr(data.FIELD_AudioVisualCanion)
                    End If

                    If Not dr.IsNull(data.FIELD_AudioVisualLapTop) Then
                        lblAudioVisualLapTop.Text = dr(data.FIELD_AudioVisualLapTop)
                    End If

                    If Not dr.IsNull(data.FIELD_AudioVisualCopiadora) Then
                        lblAudioVisualCopiadora.Text = dr(data.FIELD_AudioVisualCopiadora)
                    End If

                    'If Not dr.IsNull(data.FIELD_AudioVisualOtra) Then

                    '    lblAudioVisualOtra.Text = dr(data.FIELD_AudioVisualOtra)

                    'End If

                    'If Not dr.IsNull(data.FIELD_AudioVisualOtraCual) Then

                    '    lblAudioVisualOtraCual.Text = dr(data.FIELD_AudioVisualOtraCual)

                    'End If

                    If Not dr.IsNull(data.FIELD_AlimentosDesayunos) Then
                        lblAlimentosDesayunos.Text = dr(data.FIELD_AlimentosDesayunos)
                    End If
                    If Not dr.IsNull(data.FIELD_AlimentosComidas) Then
                        lblAlimentosComidas.Text = dr(data.FIELD_AlimentosComidas)
                    End If
                    If Not dr.IsNull(data.FIELD_AlimentosCenas) Then
                        lblAlimentosCenas.Text = dr(data.FIELD_AlimentosCenas)
                    End If

                    If Not dr.IsNull(data.FIELD_AlimentosRequiereBuffet) Then
                        If dr(data.FIELD_AlimentosRequiereBuffet) Then
                            lblAlimentosRequiere.Text = PortalCulture.GetString("00997") '"Buffet"
                        End If
                    End If

                    If Not dr.IsNull(data.FIELD_AlimentosRequiereEmplatado) Then
                        If dr(data.FIELD_AlimentosRequiereEmplatado) Then
                            lblAlimentosRequiere.Text = PortalCulture.GetString("00998") '"Emplatado"
                        End If
                    End If
                    If Not dr.IsNull(data.FIELD_AlimentosRequiereOtro) Then
                        If dr(data.FIELD_AlimentosRequiereOtro) Then
                            lblAlimentosRequiere.Text = PortalCulture.GetString("M0BT0000335") ' "Otro"

                            'If Not dr.IsNull(data.FIELD_AlimentosRequiereOtroCual) Then
                            '    lblAlimentosRequiereOtroCual.Text = dr(data.FIELD_AlimentosRequiereOtroCual)
                            'End If
                        End If

                    End If



                    If Not dr.IsNull(data.FIELD_BebidasRefresco) Then
                        chkBebidasRefresco.Checked = dr(data.FIELD_BebidasRefresco)
                    End If
                    If Not dr.IsNull(data.FIELD_BebidasCervezas) Then
                        chkBebidasCervezas.Checked = dr(data.FIELD_BebidasCervezas)
                    End If
                    If Not dr.IsNull(data.FIELD_BebidasAguasFrutas) Then
                        chkBebidasAguasFrutas.Checked = dr(data.FIELD_BebidasAguasFrutas)
                    End If
                    If Not dr.IsNull(data.FIELD_BebidasCopeo) Then
                        chkBebidasCopeo.Checked = dr(data.FIELD_BebidasCopeo)
                    End If
                    If Not dr.IsNull(data.FIELD_BebidasBotella) Then
                        chkBebidasBotella.Checked = dr(data.FIELD_BebidasBotella)
                    End If
                    If Not dr.IsNull(data.FIELD_BebidasDescorchePersona) Then
                        chkBebidasDescorchePersona.Checked = dr(data.FIELD_BebidasDescorchePersona)
                    End If
                    If Not dr.IsNull(data.FIELD_BebidasDescorcheBotella) Then
                        chkBebidasDescorcheBotella.Checked = dr(data.FIELD_BebidasDescorcheBotella)
                    End If
                    If Not dr.IsNull(data.FIELD_BebidasBarraNacional) Then
                        chkBebidasBarraNacional.Checked = dr(data.FIELD_BebidasBarraNacional)
                    End If
                    If Not dr.IsNull(data.FIELD_BebidasBarraImportada) Then
                        chkBebidasBarraImportada.Checked = dr(data.FIELD_BebidasBarraImportada)
                    End If
                    If Not dr.IsNull(data.FIELD_BebidasEnComidas) Then
                        chkBebidasEnComidas.Checked = dr(data.FIELD_BebidasEnComidas)
                    End If
                    If Not dr.IsNull(data.FIELD_BebidasEnCenas) Then
                        chkBebidasEnCenas.Checked = dr(data.FIELD_BebidasEnCenas)
                    End If

                    If Not dr.IsNull(data.FIELD_BebidasEnOtras) Then
                        chkBebidasEnOtro.Checked = dr(data.FIELD_BebidasEnOtras)
                        If Not dr.IsNull(data.FIELD_BebidasEnCual) Then
                            lblBebidasENOtroCual.Text = dr(data.FIELD_BebidasEnCual)
                        End If
                    End If


                    If Not dr.IsNull(data.FIELD_NochesTemaPalenque) Then
                        chkNochesTemaPalenque.Checked = dr(data.FIELD_NochesTemaPalenque)
                    End If
                    If Not dr.IsNull(data.FIELD_NochesTemaCasino) Then
                        chkNochesTemaCasino.Checked = dr(data.FIELD_NochesTemaCasino)
                    End If

                    If Not dr.IsNull(data.FIELD_NochesTemaOtras) Then
                        chkNochesTemaOtras.Checked = dr(data.FIELD_NochesTemaOtras)
                        If Not dr.IsNull(data.FIELD_NochesTemaOtrasCual) Then
                            lblNochesTemaOtrasCuales.Text = dr(data.FIELD_NochesTemaOtrasCual)
                        End If
                    End If


                    If Not dr.IsNull(data.FIELD_ServiciosExternosPeleaGallos) Then
                        chkServiciosExternosPeleaGallos.Checked = dr(data.FIELD_ServiciosExternosPeleaGallos)
                    End If
                    If Not dr.IsNull(data.FIELD_ServiciosExternosMariachi) Then
                        chkServiciosExternosMariachi.Checked = dr(data.FIELD_ServiciosExternosMariachi)
                    End If
                    If Not dr.IsNull(data.FIELD_ServiciosExternosTrio) Then
                        chkServiciosExternosTrio.Checked = dr(data.FIELD_ServiciosExternosTrio)
                    End If
                    If Not dr.IsNull(data.FIELD_ServiciosExternosCarpas) Then
                        chkServiciosExternosCarpas.Checked = dr(data.FIELD_ServiciosExternosCarpas)
                    End If
                    If Not dr.IsNull(data.FIELD_ServiciosExternosManteleriaEspecial) Then
                        chkServiciosExternosManteleriaEspecial.Checked = dr(data.FIELD_ServiciosExternosManteleriaEspecial)
                    End If

                    If Not dr.IsNull(data.FIELD_ServiciosExternosCentrosMesa) Then
                        chkServiciosExternosCentroMesa.Checked = dr(data.FIELD_ServiciosExternosCentrosMesa)
                    End If
                    If Not dr.IsNull(data.FIELD_ServiciosExternosEquipoAudiovisual) Then
                        chkServiciosExternosEquipoAudioVisual.Checked = dr(data.FIELD_ServiciosExternosEquipoAudiovisual)
                    End If
                    If Not dr.IsNull(data.FIELD_ServiciosExternosRecorridoPorCiudad) Then
                        chkServiciosExternosRecorridoPorCiudad.Checked = dr(data.FIELD_ServiciosExternosRecorridoPorCiudad)
                    End If
                    If Not dr.IsNull(data.FIELD_ServiciosExternosOtros) Then
                        chkServiciosExternosOtros.Checked = dr(data.FIELD_ServiciosExternosOtros)

                    End If


                    If Not dr.IsNull(data.FIELD_EventoPorAgenciaViajes) Then
                        rbEventoPorAgenciaViajesSi.Checked = dr(data.FIELD_EventoPorAgenciaViajes)
                        If rbEventoPorAgenciaViajesSi.Checked Then

                        End If

                    End If


                    If Not dr.IsNull(data.FIELD_CotizadoEnPaquete) Then
                        rbCotizadoEnPaquete.Checked = dr(data.FIELD_CotizadoEnPaquete)
                    End If

                    If Not dr.IsNull(data.FIELD_CotizadoEnHabitacionesAlimentosSeparado) Then
                        rbCotizadoEnHabitacionesAlimentos.Checked = dr(data.FIELD_CotizadoEnHabitacionesAlimentosSeparado)
                    End If

                    If Not dr.IsNull(data.FIELD_CotizadoEnOtro) Then
                        rbCotizadoEnOtro.Checked = dr(data.FIELD_CotizadoEnOtro)
                        If rbCotizadoEnOtro.Checked Then

                        End If
                    End If

                    If Not dr.IsNull(data.FIELD_RequerimiestosEspeciales) Then
                        lblRequerimiestosEspeciales.Text = dr(data.FIELD_RequerimiestosEspeciales)
                    End If
                    If Not dr.IsNull(data.FIELD_ContactoNombre) Then
                        lblContactoNombre.Text = dr(data.FIELD_ContactoNombre)
                    End If
                    If Not dr.IsNull(data.FIELD_ContactoEmpresa) Then
                        lblContactoEmpresa.Text = dr(data.FIELD_ContactoEmpresa)
                    End If
                    If Not dr.IsNull(data.FIELD_ContactoCalle) Then
                        lblContactoCalle.Text = dr(data.FIELD_ContactoCalle)
                    End If
                    If Not dr.IsNull(data.FIELD_ContactoTelefono) Then
                        lblContactoTelefono.Text = dr(data.FIELD_ContactoTelefono)
                    End If
                    If Not dr.IsNull(data.FIELD_ContactoFax) Then
                        lblContactoFax.Text = dr(data.FIELD_ContactoFax)
                    End If
                    If Not dr.IsNull(data.FIELD_ContactoPuesto) Then
                        lblContactoPuesto.Text = dr(data.FIELD_ContactoPuesto)
                    End If
                    If Not dr.IsNull(data.FIELD_ContactoCiudad) Then
                        lblContactoCiudad.Text = dr(data.FIELD_ContactoCiudad)
                    End If
                    If Not dr.IsNull(data.FIELD_ContactoColonia) Then
                        lblContactoColonia.Text = dr(data.FIELD_ContactoColonia)
                    End If
                    If Not dr.IsNull(data.FIELD_ContactoCodigoPostal) Then
                        lblContactoCodigoPostal.Text = dr(data.FIELD_ContactoCodigoPostal)
                    End If
                    If Not dr.IsNull(data.FIELD_ContactoEmail) Then
                        lblContactoEmail.Text = dr(data.FIELD_ContactoEmail)
                    End If





                End If
            End If
        Catch
            TablaPincipal.Visible = True
        End Try
    End Sub

#End Region

#Region "Event's of Control's"

    Private Sub grid_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grid.ItemCommand
        grid.SelectedIndex = -1
        If e.CommandName <> "Page" Then
            Dim id As Double = CType(e.Item.Cells(columnas.idConvenciones).Text(), Double)
            TablaPincipal.Visible = False
            Select Case e.CommandName
                Case "Edit"
                    SelectData(id)

                    grid.EditItemIndex = e.Item.ItemIndex
                    LoadGrid()
                Case "Select"
                    grid.EditItemIndex = -1
                    SelectData(id)
                    LoadGrid()

                Case "Update"
                    Dim ddl As DropDownList = CType(e.Item.FindControl("ddlEstatusEdit"), DropDownList)
                    grid.EditItemIndex = -1
                    If Not ddl Is Nothing AndAlso Not ddl.SelectedItem Is Nothing Then
                        Dim manager As ConventionsMeetingsFacade = New ConventionsMeetingsFacade
                        manager.UpdateStatusConventionsMeetings(id, CType(ddl.SelectedItem.Value, Integer))
                    End If
                    LoadDataSource()
                    LoadGrid()
                Case "Cancel"
                    grid.EditItemIndex = -1
                    LoadGrid()
            End Select
        End If
    End Sub

    Private Sub grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemDataBound

        Select Case e.Item.ItemType
            Case ListItemType.Header

            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim lnk As LinkButton = CType(e.Item.FindControl("lnkStatus"), LinkButton)
                Dim lbl As Label = CType(e.Item.FindControl("lblEstatusEdit"), Label)
                lnk.Text = PortalCulture.GetString("M000365") ''edit
                Select Case e.Item.Cells(columnas.Estatus).Text
                    Case "0"  'Nuevo
                        lbl.Text = PortalCulture.GetString("M000058") '
                    Case "1"  'proceso
                        lbl.Text = PortalCulture.GetString("00439") '
                    Case "2"  'cerrado
                        lbl.Text = PortalCulture.GetString("00151") '
                    Case "3"  'eliminado
                        lbl.Text = PortalCulture.GetString("M000059")  '
                End Select

                lnk = e.Item.FindControl("lnkSeleccionar")
                lnk.Text = PortalCulture.GetString("M000640") '

            Case ListItemType.EditItem
                Dim ddl As DropDownList = CType(e.Item.FindControl("ddlEstatusEdit"), DropDownList)
                LoadTypeStutus(ddl, True)
        End Select
    End Sub

    Private Sub grid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grid.PageIndexChanged
        Try
            grid.CurrentPageIndex = e.NewPageIndex
            grid.SelectedIndex = -1
            LoadGrid()
        Catch
        End Try
    End Sub

    Private Sub btnLoadStatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadStatus.Click
        LoadGrid()
    End Sub

    

    Private Sub grid_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(columnas.ContactoNombre).Text = PortalCulture.GetString("00432")
            e.Item.Cells(columnas.ContactoEmail).Text = PortalCulture.GetString("00726")
            e.Item.Cells(columnas.Llegada).Text = PortalCulture.GetString("M000078")
            e.Item.Cells(columnas.Salida).Text = PortalCulture.GetString("M000079")
            e.Item.Cells(columnas.Estatus).Text = PortalCulture.GetString("M000562")
        End If
    End Sub

#End Region

#Region "Propertys"

    Private Property DataSource()
        Get
            Try
                Return Session(Me.Page.ClientID + "DataSource")
            Catch
                Return Nothing
            End Try
        End Get
        Set(ByVal Value)
            Session(Me.Page.ClientID + "DataSource") = Value
        End Set
    End Property

#End Region


End Class


