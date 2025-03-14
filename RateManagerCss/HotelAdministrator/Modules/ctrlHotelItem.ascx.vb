Imports Portal.General.Common.Data.HotelItemData
Imports Portal.General.Common.Data
Partial Public Class ctrlHotelItem
    Inherits System.Web.UI.UserControl

    Dim _company As Integer = -1
    Private Const KEY_IDHOTEL As String = "idHotel"
    Public Property Moneda() As String
        Get
            Return lblCurrency.Text
        End Get
        Set(ByVal value As String)
            lblCurrency.Text = value
        End Set
    End Property

    Public Property MonedaInfo() As String
        Get
            Return lblCurrencyInfo.Text
        End Get
        Set(ByVal value As String)
            lblCurrencyInfo.Text = value
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
    Private Property IDDICCNAME() As Integer
        Get
            Return ViewState("IDDICCNAME")
        End Get
        Set(ByVal Value As Integer)
            ViewState("IDDICCNAME") = Value
        End Set
    End Property
    Private Property IDDICCDESCRIPTION() As Integer
        Get
            Return ViewState("IDDICCDESCRIPTION")
        End Get
        Set(ByVal Value As Integer)
            ViewState("IDDICCDESCRIPTION") = Value
        End Set
    End Property


    Public Property idHotel() As Integer
        Get
            If ViewState.Item(KEY_IDHOTEL) Is Nothing Then
                Return 0
            Else
                Return ViewState.Item(KEY_IDHOTEL)
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState.Add(KEY_IDHOTEL, Value)
        End Set
    End Property

    Public Property idCompany() As Integer
        Get
            If ViewState.Item("idCompany") Is Nothing Then
                Return 0
            Else
                Return ViewState.Item("idCompany")
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState.Add("idCompany", Value)
        End Set
    End Property


    Protected WithEvents ctrlImgHotelItem1 As ctrlImagesHotelItem

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            ctrlImgHotelItem1.IdEmpresa = Me.idCompany
            ctrlImgHotelItem1.ModeView = Opciones.ViewMode.Edit
            ctrlImgHotelItem1.IdIdioma = PortalCulture.GetIDCulture
        End If

        chkPaymentDestination.Enabled = False
        chkComisionable.Enabled = False
        Me.ctrlNameIdioma.IsMultiline = False

    End Sub

    Public Function SaveHotelItem(ByVal idHotel As Integer) As Integer
        If Not Page.IsValid Then Return -1


        Dim res As Boolean
        Dim ds As New HotelItemData
        Dim drow As DataRow = ds.Tables(HOTELITEM_TABLE).NewRow()

        drow(FIELD_IDHOTELITEM) = IDHotelItem
        drow(FIELD_IDHOTEL) = idHotel
        drow(FIELD_NAME) = ctrlNameIdioma.textodefault 'txtName.Text
        drow(FIELD_DESCRIPTION) = ctrlDescriptionIdioma.textodefault
        drow(FIELD_PRICE) = txtPrice.Text
        drow(FIELD_IDMONEDA) = IDMoneda
        drow(FIELD_ACTIVE) = chkActive.Checked
        drow(FIELD_COMISIONABLE) = chkComisionable.Checked
        drow(FIELD_ALLOWPAYMENTDESTINATION) = chkPaymentDestination.Checked
        drow(FIELD_TAX) = txtTax.Text
        drow(FIELD_CURRENCYCODE) = Me.Moneda
        drow(FIELD_IDDICCNAME) = ctrlNameIdioma.IdIndice
        drow(FIELD_IDDICCDESCRIPTION) = ctrlDescriptionIdioma.IdIndice


        ds.Tables(HOTELITEM_TABLE).Rows.Add(drow)

        With New Portal.General.Facade.HotelItemFacade()
            If IsEdit Then


                If Me.IDDICCNAME <> 0 Then
                    'Actualizo Diccionario
                    ctrlNameIdioma.Update(Me.IDDICCNAME, False)
                Else
                    'Nuevos IDS De Diccionario por si no el diccionario ID es nulo en bd
                    Me.IDDICCNAME = ctrlNameIdioma.Insert()
                End If

                If Me.IDDICCDESCRIPTION <> 0 Then
                    ctrlDescriptionIdioma.Update(Me.IDDICCDESCRIPTION, False)
                Else
                    Me.IDDICCDESCRIPTION = ctrlDescriptionIdioma.Insert()
                End If

                If Me.IDDICCNAME <> 0 Then
                    ds.Tables(HOTELITEM_TABLE).Rows(0).Item(HotelItemData.FIELD_IDDICCNAME) = Me.IDDICCNAME
                End If

                If Me.IDDICCDESCRIPTION <> 0 Then
                    ds.Tables(HOTELITEM_TABLE).Rows(0).Item(HotelItemData.FIELD_IDDICCDESCRIPTION) = Me.IDDICCDESCRIPTION
                End If


                drow.AcceptChanges()
                drow.SetModified()
                res = .UpdateHotelItem(ds)

                If res Then
                    ctrlImgHotelItem1.SaveImages()
                End If

            Else
                'NUEVO ITEM
                res = .InsertHotelItem(ds)

                If res Then
                    'Guardar Diccionario
                    ctrlNameIdioma.Update(ds.Tables(HotelItemData.HOTELITEM_TABLE).Rows(0)(HotelItemData.FIELD_IDDICCNAME), False)
                    ctrlDescriptionIdioma.Update(ds.Tables(HotelItemData.HOTELITEM_TABLE).Rows(0)(HotelItemData.FIELD_IDDICCDESCRIPTION), False)

                    'Guardar Imagenes
                    ctrlImgHotelItem1.IdHotelItem = ds.Tables(HotelItemData.HOTELITEM_TABLE).Rows(0)(HotelItemData.FIELD_IDHOTELITEM).ToString()
                    ctrlImgHotelItem1.SaveImages()
                End If

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

        Dim asdf As DataSet = New Portal.General.Facade.HotelItemFacade().GetHotelItem(id)

        With New Portal.General.Facade.HotelItemFacade().GetHotelItem(id)


            If .Tables(HOTELITEM_TABLE).Rows.Count > 0 Then
                IDHotelItem = id

                If Not IsDBNull(.Tables(HOTELITEM_TABLE)(0)(FIELD_PRICE)) Then
                    txtPrice.Text = Format(.Tables(HOTELITEM_TABLE)(0)(FIELD_PRICE), "###0.00")
                    txtPriceTaxInfo.Text = txtPrice.Text
                Else
                    txtPrice.Text = ""
                End If

                If Not IsDBNull(.Tables(HOTELITEM_TABLE)(0)(FIELD_ACTIVE)) Then
                    chkActive.Checked = CType(.Tables(HOTELITEM_TABLE)(0)(FIELD_ACTIVE), Boolean)
                Else
                    chkActive.Checked = False
                End If

                If Not IsDBNull(.Tables(HOTELITEM_TABLE)(0)(FIELD_COMISIONABLE)) Then
                    chkComisionable.Checked = CType(.Tables(HOTELITEM_TABLE)(0)(FIELD_COMISIONABLE), Boolean)
                Else
                    chkComisionable.Checked = False
                End If

                If Not IsDBNull(.Tables(HOTELITEM_TABLE)(0)(FIELD_ALLOWPAYMENTDESTINATION)) Then
                    chkPaymentDestination.Checked = CType(.Tables(HOTELITEM_TABLE)(0)(FIELD_ALLOWPAYMENTDESTINATION), Boolean)
                Else
                    chkPaymentDestination.Checked = False
                End If

                If Not IsDBNull(.Tables(HOTELITEM_TABLE)(0)(FIELD_TAX)) Then
                    txtTax.Text = Format(.Tables(HOTELITEM_TABLE)(0)(FIELD_TAX), "###0.00")
                    txtTaxSrc.Value = txtTax.Text

                    Dim taxD As Double = Convert.ToDouble(txtTax.Text)
                    Dim priceD As Double = Convert.ToDouble(txtPrice.Text)

                    Dim toAdd As Double = Convert.ToDouble(((taxD / 100) * priceD))

                    txtPriceTaxInfo.Text = Format((priceD + toAdd), "###0.00")

                Else
                    txtTax.Text = ""
                    txtTaxSrc.Value = txtTax.Text
                End If

                If Not IsDBNull(.Tables(HOTELITEM_TABLE)(0)(FIELD_IDDICCNAME)) Then
                    Me.IDDICCNAME = CType(.Tables(HOTELITEM_TABLE)(0)(FIELD_IDDICCNAME), Integer)
                Else
                    Me.IDDICCNAME = 0
                End If

                If Not IsDBNull(.Tables(HOTELITEM_TABLE)(0)(FIELD_IDDICCDESCRIPTION)) Then
                    Me.IDDICCDESCRIPTION = CType(.Tables(HOTELITEM_TABLE)(0)(FIELD_IDDICCDESCRIPTION), Integer)
                Else
                    Me.IDDICCDESCRIPTION = 0
                End If

                ctrlNameIdioma.CargaDatos(Me.IDDICCNAME)
                ctrlDescriptionIdioma.CargaDatos(Me.IDDICCDESCRIPTION)

                ctrlImgHotelItem1.IdEmpresa = Me.idCompany
                ctrlImgHotelItem1.ModeView = Opciones.ViewMode.Edit
                ctrlImgHotelItem1.IdIdioma = PortalCulture.GetIDCulture

                ctrlImgHotelItem1.IdHotelItem = CType(IDHotelItem, String)
                ctrlImgHotelItem1.LoadImagesByHotelItem(CType(ctrlImgHotelItem1.IdHotelItem, Integer))

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

                Dim path As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & ConfigurationManager.AppSettings("Albums_Dir") & "\" & ConfigurationManager.AppSettings("idRubro") & "\" & Me.idCompany & "\" & "Item\" & id

                If IO.Directory.Exists(path) Then
                    IO.Directory.Delete(path, True)
                End If

                ResetForm()
                    Return 1
                Else
                    'lblMsg.Text = "No se pudo eliminar el metodo de pago"
                    Return -4
            End If
        End With



    End Function

    Public Function ResetForm()

        txtPrice.Text = String.Empty
        txtTax.Text = String.Empty
        txtPriceTaxInfo.Text = String.Empty
        chkActive.Checked = False
        chkComisionable.Checked = False
        chkPaymentDestination.Checked = True
        ctrlImgHotelItem1.IdHotelItem = "0"
        ctrlNameIdioma.Limpia()
        ctrlDescriptionIdioma.Limpia()
        ctrlImgHotelItem1.LoadImagesByHotelItem(CType(ctrlImgHotelItem1.IdHotelItem, Integer))
        IsEdit = False
    End Function

    Private Sub LoadResources()
        lblDescripcion.Text = PortalCulture.GetString("M000152", True)
        lblName.Text = PortalCulture.GetString("00073", True)
        lblPrice.Text = PortalCulture.GetString("01684", True)
        lblPriceWithTaxInfo.Text = PortalCulture.GetString("01685", True)
        lblActive.Text = PortalCulture.GetString("01680", False)
        lblPaymentDestination.Text = PortalCulture.GetString("01681", False)
        lblTax.Text = PortalCulture.GetString("01682", True)
        lblComisionable.Text = PortalCulture.GetString("01683", False)
        RequiredFieldValidator8.Text = PortalCulture.GetString("M0UT00502")
        RangeValidator9.Text = PortalCulture.GetString("M0UT00503")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        LoadResources()
    End Sub

End Class