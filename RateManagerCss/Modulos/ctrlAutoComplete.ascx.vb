Public Partial Class ctrlAutoComplete
    Inherits System.Web.UI.UserControl

    Public Event OnSendFilter(ByVal id As String, ByVal descripcion As String)

    Public ReadOnly Property GetFilter() As String
        Get
            Return inpuFilter.Value
        End Get
    End Property
    Public Property CausesValidation() As Boolean
        Set(ByVal value As Boolean)
            ImgSaveSearch.CausesValidation = value
        End Set
        Get
            Return ImgSaveSearch.CausesValidation
        End Get
    End Property
    Public Property ValidationGroup() As String
        Set(ByVal value As String)
            ImgSaveSearch.ValidationGroup = value
        End Set
        Get
            Return ImgSaveSearch.ValidationGroup
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim txt As TextBox
        If Not Me.IsPostBack Then
            If String.IsNullOrEmpty(Me.autocomplete.Value) AndAlso String.IsNullOrEmpty(Me.inpuFilter.Value) Then

            End If
        End If

        lnkSearch.Attributes.Add("onclick", "javascript:FireCrearAutoComplete();")
        lnkCancel.Attributes.Add("onclick", "javascript:FireHlnkClose();")
        lnkFilter.Attributes.Add("onclick", String.Format("javascript:FireShowCtrlFilter('{0}');", DivRadioButtons.ClientID))
        rb1.Attributes.Add("onclick", String.Format("javascript:FireChangeIndex(0,'{0}','{1}','{2}');", DivRadioButtons.ClientID, spanHeader.ClientID, RateManager.PortalCulture.GetString("M0BT0000113")))
        rb2.Attributes.Add("onclick", String.Format("javascript:FireChangeIndex(1,'{0}','{1}','{2}');", DivRadioButtons.ClientID, spanHeader.ClientID, RateManager.PortalCulture.GetString("M0BT0000113")))
    End Sub

    'Protected Sub ImgSaveNoPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgSaveNoPrint.Click
    '    'RaiseEvent onSendFilter(Me, )
    'End Sub
    Protected Sub ImgSaveSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgSaveSearch.Click
        Dim sFiltro As String = ""
        Dim skey As String()
        Dim ID As Integer

        skey = Me.idAutocompleteFilter.Value.Split("|")
        If IsNumeric(idCliente.Value) AndAlso idCliente.Value <> 0 Then
            sFiltro = String.Format("{0} = '{1}' ", skey(0), idCliente.Value)
        Else
            If Not String.IsNullOrEmpty(Me.autocomplete.Value) Then
                If rb1.Checked Then
                    sFiltro = String.Format("{0} like '%{1}%' ", skey(1), Me.autocomplete.Value.Trim)
                ElseIf rb2.Checked Then
                    sFiltro = String.Format("{0} like '%{1}%' ", skey(2), Me.autocomplete.Value.Trim)
                End If
            End If
        End If

        inpuFilter.Value = sFiltro
        RaiseEvent OnSendFilter(Me.idCliente.Value, sFiltro)
        idCliente.Value = "0"

    End Sub

End Class