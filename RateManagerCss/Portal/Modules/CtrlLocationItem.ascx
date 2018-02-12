<%@ Control Language="vb" ClassName="CtrlLocationItem" AutoEventWireup="true" Inherits="System.Web.UI.UserControl" %>
<%@ Import Namespace="RateManager" %>

<script runat="server">
    Private _type As LocationType
    Private _childControl As CtrlLocationItem
    
    Public Enum LocationType
        Unknow = 0
        Country = 1
        State = 2
        District = 3
        City = 4
        Area = 5
    End Enum
    
    Public Property Type() As LocationType
        Get
            Return Me._type
        End Get
        Set(ByVal value As LocationType)
            Me._type = value
        End Set
    End Property
    
    Public Property SelectedValue() As String
        Get
            Return Me.varSelected.Value
        End Get
        Set(ByVal value As String)
            Me.varSelected.Value = value
        End Set
    End Property
    
    Public Property Child() As String
        Get
            Return Me.varChild.Value
        End Get
        Set(ByVal value As String)
            Me.varChild.Value = value
        End Set
    End Property
    
    Private ReadOnly Property ChildControl() As CtrlLocationItem
        Get
            If Me._childControl Is Nothing AndAlso Me.Child.Trim.Length > 0 Then
                Me._childControl = Me.Page.FindControl(Me.Child)
            End If
            Return Me._childControl
        End Get
    End Property
    
    Public Overrides ReadOnly Property ClientID() As String
        Get
            Return Me.lstItems.ClientID
        End Get
    End Property
    
    Private ReadOnly Property SelectionClientID() As String
        Get
            Return Me.varSelected.ClientID
        End Get
    End Property
    
    Protected Sub ControlLoad(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

    End Sub
    
    Public Property Width() As String
        Get
            Return Me.lstItems.Style("width")
        End Get
        Set(ByVal value As String)
            Me.lstItems.Style.Add("width", value)
        End Set
    End Property
    
</script>

<% 
    Me.lstItems.Attributes.Add("class", Me.Type.ToString.ToLower())
%>
<select id="lstItems" runat="server"></select>
<input id="varChild" type="hidden" value="ddlDistrict" runat="server" />
<input id="varSelected" type="hidden" runat="server" value="" />

<script type="text/javascript">
    var _locationLoadingID = 'imgLoading';
    var _locationLoadingImage = 'images/indicator.gif';
    var _locationParameters = new Object();

    $('#<%= Me.ClientId %>').change(function() {
        if ($(this).find('option').length > 0) {
            $(this).removeAttr('disabled');
            var selected = $('#<%= Me.varSelected.ClientId %>');
            selected.val($(this).find('option:selected').attr('value'));
            <%if Me.Child.trim().length > 0 andalso Me.ChildControl isnot Nothing then %>
            var child = $('#<%= Me.ChildControl.ClientID %>');
            UpdateLoading(child, { "display": "", "position": "Right" });
            child.html('');
            $.ajax({
                url: '<%= Request.ApplicationPath %>/servicios/locationitems.ashx',
                dataType: 'json', 
                data: { type: '<%= Me.ChildControl.Type.ToString().ToLower() %>', parent: selected.val() },
                success: function(items) {
                    $.each(items, function() {
                        var item = $('<option></option>');
                        item.val(this['id']);
                        var selector = $('#<%= Me.ChildControl.SelectionClientId %>');  
                        if (selector.length != 0 && selector.val() == this['id'])
                            item.attr('selected', 'selected');
                        item.html(this['name']); 
                        child.append(item);
                    });
                },
                complete: function(result) {
                    UpdateLoading(this, { "display": "none" });
                    child.change();
                }
            });
            <%end if %>
        } else {
            $(this).attr('disabled', 'true');
        }
        

    });
        
</script> 

 
<% If Me.Type = LocationType.Country Then%>
   
<script type="text/javascript">
    $(document).ready(
        function() {
            var current = $('#<%= Me.ClientId %>');
            var selected = $('#<%= Me.varSelected.ClientId %>');
            UpdateLoading(current, { "display": "", "position": "Right" });
            current.html('');
            $.ajax({
                url: '<%= Request.ApplicationPath %>/servicios/locationitems.ashx',
                dataType: 'json',
                data: { type: 'country', lang: '<%= PortalCulture.GetIDCulture().ToString() %>' },
                success: function(items) {
                    $.each(items, function() {
                        var item = $('<option></option>');
                        item.val(this['id']);
                        if (selected && selected.val() == this['id'])
                            item.attr('selected', 'selected');
                        item.html(this['name']);
                        current.append(item);
                    });
                },
                complete: function(result) {
                    UpdateLoading(this, { "display": "none" });
                    if (selected.val().length == 0) {
                        selected.val(current.find('option:selected').attr('value'));
                    }
                    current.change();
                }
            });
        }
    );
</script>

 <% end if %>  
    
    
    
    