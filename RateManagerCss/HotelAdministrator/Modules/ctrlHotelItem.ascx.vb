Imports Portal.General.Common.Data.HotelItemData
Imports Portal.General.Common.Data
Partial Public Class ctrlHotelItem
    Inherits System.Web.UI.UserControl

    Dim _company As Integer = -1
    Public Property Moneda() As String
        Get
            Return lblCurrency.Text
        End Get
        Set(ByVal value As String)
            lblCurrency.Text = value
        End Set
    End Property
    Public Property IDMoneda() As Integer
        Get
            Return ViewState("CtrPaymentMethod_IDMoneda")
        End Get
        Set(ByVal Value As Integer)
            ViewState("CtrPaymentMethod_IDMoneda") = Value
        End Set
    End Property
    Public Property IDHotelItem() As Integer
        Get
            Return ViewState("CtrPaymentMethod_IDHotelItem")
        End Get
        Set(ByVal Value As Integer)
            ViewState("CtrPaymentMethod_IDHotelItem") = Value
        End Set
    End Property
    Public Property IsEdit() As Boolean
        Get
            Return ViewState("ctrlHotelItem_isEdit")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("ctrlHotelItem_isEdit") = Value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function SaveHotelItem(ByVal idHotel As Integer) As Integer
        If Not Page.IsValid Then Return -1


        Dim res As Boolean
        Dim ds As New HotelItemData
        Dim drow As DataRow = ds.Tables(HOTELITEM_TABLE).NewRow()

        drow(FIELD_IDHOTELITEM) = IDHotelItem
        drow(FIELD_IDHOTEL) = idHotel
        drow(FIELD_NAME) = txtName.Text
        drow(FIELD_DESCRIPTION) = txtDescription.Text
        drow(FIELD_PRICE) = txtPrice.Text
        drow(FIELD_IDMONEDA) = IDMoneda
        ds.Tables(HOTELITEM_TABLE).Rows.Add(drow)

        With New Portal.General.Facade.HotelItemFacade()
            If IsEdit Then
                drow.AcceptChanges()
                drow.SetModified()
                res = .UpdateHotelItem(ds)
            Else
                res = .InsertHotelItem(ds)
            End If
        End With

        If res Then
            IsEdit = False
            ResetForm()
            Return 1
        Else
            Return -3
        End If



    End Function

    Public Function LoadHotelItem(ByVal id As Integer, ByVal isEdit As Boolean) As Integer
        Me.IsEdit = isEdit

        With New Portal.General.Facade.HotelItemFacade().GetHotelItem(id)


            If .Tables(HOTELITEM_TABLE).Rows.Count > 0 Then
                IDHotelItem = id
                txtDescription.Text = .Tables(HOTELITEM_TABLE)(0)(FIELD_DESCRIPTION)
                txtName.Text = .Tables(HOTELITEM_TABLE)(0)(FIELD_NAME)
                If IDMoneda = .Tables(HOTELITEM_TABLE)(0)(FIELD_IDMONEDA) Then
                    txtPrice.Text = .Tables(HOTELITEM_TABLE)(0)(FIELD_PRICE)
                End If


                Return 1
            Else
                Return 0
            End If
        End With

    End Function

    Public Function DeletHotelItem(ByVal id As Integer) As Integer


        With New Portal.General.Facade.HotelItemFacade()
            If .DeleteHotelItem(id) Then
                'lblMsg.Text = "Metodo de pago eliminado"
                ResetForm()
                Return 1
            Else
                'lblMsg.Text = "No se pudo eliminar el metodo de pago"
                Return -4
            End If
        End With



    End Function

    Public Function ResetForm()
        txtDescription.Text = String.Empty
        txtName.Text = String.Empty
        txtPrice.Text = String.Empty

        IsEdit = False
    End Function

    Private Sub LoadResources()
        lblDescripcion.Text = PortalCulture.GetString("M000152", True)
        lblName.Text = PortalCulture.GetString("00073", True)
        lblPrice.Text = PortalCulture.GetString("00090", True)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        LoadResources()
    End Sub

End Class