Imports System.Data.Entity.Core.Objects

Public Class RetentionsConfig
    Inherits PaginaBase

    Protected predefined As ObjectQuery(Of IPR_MensajesPredefinidos)
    Protected PropertyNumber As Integer

    Public Property Editing() As Boolean
        Get
            Return ViewState("Editing")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Editing") = Value
        End Set
    End Property

    Private Property EditingID() As Integer
        Get
            Return ViewState("EditingID")
        End Get
        Set(ByVal Value As Integer)
            ViewState("EditingID") = Value
        End Set
    End Property

    '<summary>
    '   Punto de entrada para el panel de configuracion de la herramienta IPR.
    '</summary>
    '<param name="sender"></param>
    '<param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)

        If Not cInfoActual.Hotel = 0 Then
            PropertyNumber = cInfoActual.Hotel

            ' Cargar los elementos desde la base de datos. Si no hay un objeto de configuracion, crearlo en este momento
            ' Determinar la propiedad que se va a trabajar a traves de un Query String
            Using db As New ozunivisitEntities
                Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

                If config Is Nothing Then
                    'Crea la configuración
                    config = New IPR_Configuracion() With {
                        .id_propiedad = PropertyNumber,
                        .url = String.Empty
                    }

                    db.AddToIPR_Configuracion(config)
                    db.SaveChanges()
                End If

                If Not IsPostBack Then
                    txtUrl.Text = config.url
                    chkHabilitado.Checked = config.habilitado
                    chkComparador.Checked = config.comparador
                    chkMensajes.Checked = config.mensajes
                    chkRetencion.Checked = config.retencion

                    lblPropertyNumber.Text = ""

                    config.IPR_Mensajes.Load()
                    config.IPR_Retencion.Load()

                    gridMessages.DataSource = config.IPR_Mensajes.OrderBy(Function(x) x.orden)
                    gridMessages.DataBind()

                    predefined = db.IPR_MensajesPredefinidos

                    gridPredefinedMessages.DataSource = predefined
                    gridPredefinedMessages.DataBind()

                    gridRetentionMessages.DataSource = config.IPR_Retencion
                    gridRetentionMessages.DataBind()

                    toolDiv.Visible = chkHabilitado.Checked

                End If
            End Using
        End If
    End Sub

    Protected Sub BindData()
        Using db As New ozunivisitEntities
            Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

            config.IPR_Mensajes.Load()
            config.IPR_Sesiones.Load()

            gridMessages.DataSource = config.IPR_Mensajes.OrderBy(Function(x) x.orden)
            gridMessages.DataBind()

            predefined = db.IPR_MensajesPredefinidos

            gridPredefinedMessages.DataSource = predefined
            gridPredefinedMessages.DataBind()

            gridRetentionMessages.DataSource = config.IPR_Retencion
            gridRetentionMessages.DataBind()

        End Using
    End Sub

    Private Sub gridMessages_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gridMessages.RowDataBound
        Dim lnkEditar As LinkButton
        Dim LK2 As LinkButton
        Dim LK As HyperLink

        If e.Row.RowType = DataControlRowType.DataRow Then
            lnkEditar = DirectCast(e.Row.FindControl("lnkedit"), LinkButton)
            lnkEditar.Text = PortalCulture.GetString("00093")

            LK2 = DirectCast(e.Row.FindControl("lnkEliminar2"), LinkButton)
            LK = DirectCast(e.Row.FindControl("lnkEliminar"), HyperLink)
            LK.Text = PortalCulture.GetString("00103")
            LK.Attributes.Add("onClick", "javascript:openModal('" & LK2.ClientID & "', '" & PortalCulture.GetString("01649") & "','" & PortalCulture.GetString("01651") & "')")
        End If
    End Sub

    Private Sub gridRetentionMessages_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gridRetentionMessages.RowDataBound
        Dim lnkEditar As LinkButton
        Dim LK2 As LinkButton
        Dim LK As HyperLink

        If e.Row.RowType = DataControlRowType.DataRow Then
            lnkEditar = DirectCast(e.Row.FindControl("lnkeditMsgRet"), LinkButton)
            lnkEditar.Text = PortalCulture.GetString("00093")

            LK2 = DirectCast(e.Row.FindControl("lnkEliminarMsgRet2"), LinkButton)
            LK = DirectCast(e.Row.FindControl("lnkEliminarMsgRet"), HyperLink)
            LK.Text = PortalCulture.GetString("00103")
            LK.Attributes.Add("onClick", "javascript:openModal('" & LK2.ClientID & "', '" & PortalCulture.GetString("01649") & "','" & PortalCulture.GetString("01651") & "')")
        End If
    End Sub

    Private Sub gridMessages_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gridMessages.RowCommand
        If e.CommandName = "Select" Then
            Using db As New ozunivisitEntities()
                Editing = True
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow) 'gridMessages.Rows(gridMessages.SelectedIndex)
                EditingID = Convert.ToInt32(gridMessages.DataKeys(row.RowIndex).Value.ToString())

                ' Buscar el elemento
                Dim message As IPR_Mensajes = db.IPR_Mensajes.FirstOrDefault(Function(x) x.id_mensaje = EditingID)

                txtMessage.Text = message.mensaje
                txtDuracion.Text = message.duracion
                txtRutas.Text = message.rutas
                txtOrden.Text = message.orden
                txtDateMsgStart.Text = message.fecha_inicio
                txtDateMsgTo.Text = message.fecha_fin
                txtIcon.Text = message.icono

                divNuevo.Visible = True
                btnNuevo.Visible = False

                BindData()
            End Using
        End If
    End Sub

    Private Sub gridRetentionMessages_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gridRetentionMessages.RowCommand
        If e.CommandName = "Select" Then
            Using db As New ozunivisitEntities()
                Editing = True
                Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow) 'gridMessages.Rows(gridMessages.SelectedIndex)
                EditingID = Convert.ToInt32(gridRetentionMessages.DataKeys(row.RowIndex).Value.ToString())

                ' Buscar el elemento
                Dim message As IPR_Retencion = db.IPR_Retencion.FirstOrDefault(Function(x) x.id_retencion = EditingID)

                txtRetMessage.Text = message.mensaje
                txtRetDuration.Text = message.duracion
                txtRetRutes.Text = message.ruta
                txtRetAction.Text = message.texto_boton
                txtRetApp.Text = message.plan_tarifario
                txtRetAccessCode.Text = message.codigo_acceso
                txtRetTimes.Text = message.cuando
                txtRetTitle.Text = message.titulo

                divnuevoRetencion.Visible = True
                btnNuevoRetencion.Visible = False

                BindData()
            End Using
        End If
    End Sub

    Protected Sub chkHabilitado_CheckedChancged(ByVal sender As Object, ByVal e As EventArgs)
        Using db As New ozunivisitEntities
            Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

            config.habilitado = chkHabilitado.Checked
            db.SaveChanges()

            If chkHabilitado.Checked Then
                toolDiv.Visible = True
            Else
                toolDiv.Visible = False
            End If
        End Using
    End Sub

    Protected Sub chkComparador_CheckedChanged(sender As Object, e As EventArgs)
        Using db As New ozunivisitEntities()
            Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

            config.comparador = chkComparador.Checked
            db.SaveChanges()
        End Using
    End Sub


    Protected Sub chkMensajes_CheckedChanged(sender As Object, e As EventArgs)
        Using db As New ozunivisitEntities()
            Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

            config.mensajes = chkMensajes.Checked
            db.SaveChanges()
        End Using
    End Sub

    Protected Sub chkRetencion_CheckedChanged(sender As Object, e As EventArgs)
        Using db As New ozunivisitEntities()
            Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

            config.retencion = chkRetencion.Checked
            db.SaveChanges()
        End Using
    End Sub


    Protected Sub gridMessages_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gridMessages.EditIndex = e.NewEditIndex
        BindData()
    End Sub

    Protected Sub gridMessages_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Using db As New ozunivisitEntities()
            Dim id As Integer = Convert.ToInt32(gridMessages.DataKeys(e.RowIndex).Value.ToString())
            Dim row As GridViewRow = gridMessages.Rows(e.RowIndex)

            Dim txtMessage = DirectCast(row.Cells(2).Controls(0), TextBox)
            Dim txtFechaInicio As Calendar = DirectCast(row.Cells(3).FindControl("ModCalFechaInicio"), Calendar)
            Dim txtFechaFin As Calendar = DirectCast(row.Cells(4).FindControl("ModCalFechaFin"), Calendar)
            Dim txtOrden = DirectCast(row.Cells(5).Controls(0), TextBox)
            Dim txtRutas = DirectCast(row.Cells(6).Controls(0), TextBox)
            Dim txtDuracion = DirectCast(row.Cells(7).Controls(0), TextBox)

            ' Buscar el elemento
            Dim message As IPR_Mensajes = db.IPR_Mensajes.FirstOrDefault(Function(x) x.id_mensaje = id)

            ' Hacerle los cambios al objeto
            message.mensaje = txtMessage.Text

            If Not String.IsNullOrEmpty(txtFechaFin.SelectedDate.ToShortDateString) Then
                message.fecha_fin = DateTime.Parse(txtFechaFin.SelectedDate.ToShortDateString)
            End If

            If Not String.IsNullOrEmpty(txtFechaInicio.SelectedDate.ToShortDateString) Then
                message.fecha_inicio = DateTime.Parse(txtFechaInicio.SelectedDate.ToShortDateString)
            End If

            message.orden = Integer.Parse(txtOrden.Text)
            message.rutas = txtRutas.Text
            message.duracion = Integer.Parse(txtDuracion.Text)

            db.SaveChanges()

            gridMessages.EditIndex = -1
            BindData()
        End Using
    End Sub


    Protected Sub gridPredefinedMessages_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gridPredefinedMessages.EditIndex = e.NewEditIndex
        BindData()
    End Sub

    Protected Sub gridPredefinedMessages_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Using db As New ozunivisitEntities()
            Dim id As Integer = Convert.ToInt32(gridPredefinedMessages.DataKeys(e.RowIndex).Value.ToString())
            Dim row As GridViewRow = gridPredefinedMessages.Rows(e.RowIndex)

            Dim txtMessage As TextBox = DirectCast(row.Cells(1).Controls(0), TextBox)

            ' Buscar el elemento
            Dim message = db.IPR_MensajesPredefinidos.FirstOrDefault(Function(x) x.id_mensaje = id)

            message.mensaje = txtMessage.Text

            db.SaveChanges()

            gridPredefinedMessages.EditIndex = -1
            BindData()
        End Using
    End Sub


    Protected Sub gridPredefinedMessages_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gridPredefinedMessages.EditIndex = -1
        BindData()
    End Sub

    Protected Sub gridMessages_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gridMessages.EditIndex = -1
        BindData()
    End Sub

    Protected Sub gridPredefinedMessages_RowCommand(sender As Object, e As GridViewCommandEventArgs)
    End Sub

    Protected Sub chkHabilitado_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Using db As New ozunivisitEntities()
            Dim checkBox As CheckBox = CType(sender, CheckBox)
            Dim row As GridViewRow = checkBox.Parent.Parent

            Dim id As Integer = CInt(gridPredefinedMessages.DataKeys(row.RowIndex).Value.ToString())
            Dim tipo As Integer = db.IPR_MensajesPredefinidos.FirstOrDefault(Function(x) x.id_mensaje = id).tipo
            Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

            Dim predefinedMessages As List(Of String)

            If config.mensajes_predefinidos IsNot Nothing Then
                predefinedMessages = config.mensajes_predefinidos.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries).ToList()
            Else
                predefinedMessages = New List(Of String)
            End If

            If Not checkBox.Checked Then
                predefinedMessages.Remove(tipo.ToString())
            Else
                If Not predefinedMessages.Contains(tipo.ToString()) Then
                    predefinedMessages.Add(tipo.ToString())
                End If
            End If

            'Actualizar el campo de los mensajes predefinidos
            Dim predefinedStr As String = String.Join(",", predefinedMessages.ToArray())
            config.mensajes_predefinidos = predefinedStr

            db.SaveChanges()

        End Using
    End Sub

    Protected Sub chkHabilitado_OnPreRender(ByVal sender As Object, ByVal e As EventArgs)
        'Establecer el valor del checkbox siempre y cuando tengamos habilitado el mensaje en la base de datos
        Using db As New ozunivisitEntities
            Dim checkBox As CheckBox = CType(sender, CheckBox)
            Dim row As GridViewRow = CType(checkBox.Parent.Parent, GridViewRow)

            Dim id As Integer = CInt(gridPredefinedMessages.DataKeys(row.RowIndex).Value.ToString())
            Dim mensajes As IPR_MensajesPredefinidos = db.IPR_MensajesPredefinidos.FirstOrDefault(Function(x) x.id_mensaje = id)

            Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

            Dim predefinedMessages As List(Of String)

            If config.mensajes_predefinidos IsNot Nothing Then
                predefinedMessages = config.mensajes_predefinidos.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries).ToList()
            Else
                predefinedMessages = New List(Of String)
            End If

            If predefinedMessages Is Nothing Then
                Return
            End If

            checkBox.Checked = predefinedMessages.Contains(mensajes.tipo.ToString())

            If mensajes.tipo = 6 Then
                checkBox.Visible = False
            End If
        End Using
    End Sub

    Protected Sub btnNuevo_OnClick(ByVal sender As Object, ByVal e As EventArgs)
        divNuevo.Visible = True
        btnNuevo.Visible = False
    End Sub

    Protected Sub btnNuevoCancelar_OnClick(ByVal sender As Object, ByVal e As EventArgs)
        ClrMessage()
    End Sub

    Protected Sub btnCancelarRetencion_OnClick(ByVal sender As Object, ByVal e As EventArgs)
        ClrMsgRetention()
    End Sub

    Protected Sub btnNuevoGuardar_OnClick(sender As Object, e As EventArgs)
        Try
            If Editing = False Then
                Using db As New ozunivisitEntities
                    Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

                    Dim newMessage As IPR_Mensajes = New IPR_Mensajes() With {
                         .IPR_Configuracion = config,
                         .duracion = Convert.ToInt32(txtDuracion.Text),
                         .orden = Convert.ToInt32(txtOrden.Text),
                         .mensaje = txtMessage.Text,
                         .rutas = txtRutas.Text,
                         .icono = txtIcon.Text
                    }

                    If txtDateMsgStart.Text <> "" Then
                        newMessage.fecha_inicio = txtDateMsgStart.Text
                    End If

                    If txtDateMsgTo.Text <> DateTime.MinValue Then
                        newMessage.fecha_fin = txtDateMsgTo.Text
                    End If

                    db.AddToIPR_Mensajes(newMessage)
                    db.SaveChanges()
                End Using
            Else
                Using db As New ozunivisitEntities()
                    ' Buscar el elemento
                    Dim message As IPR_Mensajes = db.IPR_Mensajes.FirstOrDefault(Function(x) x.id_mensaje = EditingID)

                    ' Hacerle los cambios al objeto
                    message.mensaje = txtMessage.Text

                    If Not String.IsNullOrEmpty(txtDateMsgTo.Text) Then
                        message.fecha_fin = DateTime.Parse(txtDateMsgTo.Text)
                    End If

                    If Not String.IsNullOrEmpty(txtDateMsgStart.Text) Then
                        message.fecha_inicio = DateTime.Parse(txtDateMsgStart.Text)
                    End If

                    message.orden = Integer.Parse(txtOrden.Text)
                    message.rutas = txtRutas.Text
                    message.duracion = Integer.Parse(txtDuracion.Text)
                    message.icono = txtIcon.Text

                    db.SaveChanges()

                    Editing = False
                    EditingID = 0
                    gridMessages.SelectedIndex = -1

                End Using
            End If
            ClrMessage()
        Catch ex As Exception
            lblMessageError.Text = "Mensaje para el Administrador: " & ex.Message
            lblMessageError.Visible = True
        End Try
        BindData()
    End Sub

    Private Sub ClrMessage()
        txtMessage.Text = ""
        txtDateMsgStart.Text = ""
        txtDateMsgTo.Text = ""
        txtOrden.Text = ""
        txtRutas.Text = ""
        txtDuracion.Text = ""
        txtIcon.Text = ""
        divNuevo.Visible = False
        btnNuevo.Visible = True
        gridMessages.SelectedIndex = -1
    End Sub

    Protected Sub btnNuevoGuardarRetencion_OnClick(sender As Object, e As EventArgs)
        Try
            If Not Editing Then
                Using db As New ozunivisitEntities
                    Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

                    Dim newRetentionMessage As IPR_Retencion = New IPR_Retencion() With {
                        .IPR_Configuracion = config,
                        .titulo = txtRetTitle.Text,
                        .mensaje = txtRetMessage.Text,
                        .plan_tarifario = txtRetApp.Text,
                        .ruta = txtRetRutes.Text,
                        .texto_boton = txtRetAction.Text,
                        .cuando = txtRetTimes.Text,
                        .codigo_acceso = txtRetAccessCode.Text,
                        .duracion = txtRetDuration.Text
                        }

                    db.AddToIPR_Retencion(newRetentionMessage)
                    db.SaveChanges()

                    divnuevoRetencion.Visible = False
                    btnNuevoRetencion.Visible = True
                End Using
            Else
                Using db As New ozunivisitEntities()
                    ' Buscar el elemento
                    Dim message As IPR_Retencion = db.IPR_Retencion.FirstOrDefault(Function(x) x.id_retencion = EditingID)

                    ' Hacerle los cambios al objeto
                    message.mensaje = txtRetMessage.Text
                    message.titulo = txtRetTitle.Text
                    message.texto_boton = txtRetAction.Text
                    message.cuando = CInt(txtRetTimes.Text)
                    message.duracion = CInt(txtRetDuration.Text)
                    message.plan_tarifario = txtRetApp.Text
                    message.codigo_acceso = txtRetAccessCode.Text
                    message.ruta = txtRetRutes.Text

                    db.SaveChanges()

                    Editing = False
                    EditingID = 0
                    gridRetentionMessages.SelectedIndex = -1

                End Using
            End If
            ClrMsgRetention()

        Catch ex As Exception
            lblRetError.Text = ex.Message
            lblRetError.Visible = True
        End Try

        BindData()
    End Sub

    Private Sub ClrMsgRetention()
        txtRetAccessCode.Text = ""
        txtRetAction.Text = ""
        txtRetApp.Text = ""
        txtRetDuration.Text = ""
        txtRetRutes.Text = ""
        txtRetMessage.Text = ""
        txtRetTimes.Text = ""
        txtRetTitle.Text = ""

        'btnNuevoRetencion.Visible = True
        divnuevoRetencion.Visible = False

        gridRetentionMessages.SelectedIndex = -1
    End Sub

    Protected Sub chkHabilitado_CheckedChanged(sender As Object, e As EventArgs)
        Using db As New ozunivisitEntities
            Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

            config.habilitado = chkHabilitado.Checked

            db.SaveChanges()

            If chkHabilitado.Checked Then
                toolDiv.Visible = True
            Else
                toolDiv.Visible = False
            End If
        End Using
    End Sub

    Protected Sub gridMessages_OnRowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Using db As New ozunivisitEntities
            Dim id As Integer = Convert.ToInt32(gridMessages.DataKeys(e.RowIndex).Value.ToString())
            Dim row As GridViewRow = gridMessages.Rows(e.RowIndex)

            Dim message As IPR_Mensajes = db.IPR_Mensajes.FirstOrDefault(Function(x) x.id_mensaje = id)
            db.DeleteObject(message)
            db.SaveChanges()
        End Using
        BindData()
    End Sub

    Protected Sub gridRetentionMessages_OnRowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Using db As New ozunivisitEntities
            Dim id As Integer = Convert.ToInt32(gridRetentionMessages.DataKeys(e.RowIndex).Value.ToString())
            Dim row As GridViewRow = gridRetentionMessages.Rows(e.RowIndex)

            Dim message As IPR_Retencion = db.IPR_Retencion.FirstOrDefault(Function(x) x.id_retencion = id)
            db.DeleteObject(message)
            db.SaveChanges()
        End Using
        BindData()
    End Sub

    Protected Sub btnNuevoRetencion_OnClick(sender As Object, e As EventArgs)
        divnuevoRetencion.Visible = True
        BindData()
    End Sub

    Protected Sub btnNuevoCancelarRetencion_OnClick(sender As Object, e As EventArgs)
        ClrMsgRetention()
        BindData()
    End Sub

    Protected Sub gridRetentionMessages_OnRowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Using db As New ozunivisitEntities()
            Dim id As Integer = Convert.ToInt32(gridRetentionMessages.DataKeys(e.RowIndex).Value.ToString())
            Dim row As GridViewRow = gridRetentionMessages.Rows(e.RowIndex)

            Dim txtTitle As TextBox = DirectCast(row.Cells(1).Controls(0), TextBox)
            Dim txtMessage As TextBox = DirectCast(row.Cells(2).Controls(0), TextBox)
            Dim txtCallAction As TextBox = DirectCast(row.Cells(3).Controls(0), TextBox)
            Dim txtRute As TextBox = DirectCast(row.Cells(4).Controls(0), TextBox)
            Dim txtTimes As TextBox = DirectCast(row.Cells(5).Controls(0), TextBox)
            Dim txtDuration As TextBox = DirectCast(row.Cells(6).Controls(0), TextBox)
            Dim txtEngine As TextBox = DirectCast(row.Cells(7).Controls(0), TextBox)
            Dim txtAccessCode As TextBox = DirectCast(row.Cells(8).Controls(0), TextBox)

            ' Buscar el elemento
            Dim message As IPR_Retencion = db.IPR_Retencion.FirstOrDefault(Function(x) x.id_retencion = id)

            message.titulo = txtTitle.Text
            message.mensaje = txtMessage.Text
            message.texto_boton = txtCallAction.Text
            message.ruta = txtRute.Text
            message.cuando = txtTimes.Text
            message.duracion = txtDuration.Text
            message.plan_tarifario = txtEngine.Text
            message.codigo_acceso = txtAccessCode.Text

            db.SaveChanges()

            gridRetentionMessages.EditIndex = -1
        End Using

        BindData()
    End Sub

    Protected Sub gridRetentionMessages_OnRowEditing(sender As Object, e As GridViewEditEventArgs)
        gridRetentionMessages.EditIndex = e.NewEditIndex
        BindData()
    End Sub

    Protected Sub gridRetentionMessages_OnRowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gridRetentionMessages.EditIndex = -1
        BindData()
    End Sub

    Protected Sub btnUpdateURL_Click(sender As Object, e As EventArgs)
        Using db As New ozunivisitEntities
            Dim config As IPR_Configuracion = db.IPR_Configuracion.FirstOrDefault(Function(x) x.id_propiedad = PropertyNumber)

            config.url = txtUrl.Text

            db.SaveChanges()

            If chkHabilitado.Checked Then
                toolDiv.Visible = True
            Else
                toolDiv.Visible = False
            End If
        End Using
    End Sub
End Class