'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class ctrRatePlan

    '''<summary>
    '''Control strError.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents strError As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control lblRatecode.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRatecode As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtRateCode.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtRateCode As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control RfvRateCode.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RfvRateCode As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control lblMoneda.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblMoneda As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control cmbMonedas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbMonedas As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lblhelp.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblhelp As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblname.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblname As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblDescripcion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDescripcion As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblSegmento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblSegmento As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddlSegmentos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlSegmentos As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lblRule.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRule As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddlRules.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlRules As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lblAsignarPais.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblAsignarPais As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control chklCountries.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chklCountries As Global.System.Web.UI.WebControls.CheckBoxList

    '''<summary>
    '''Control lstRatePlans.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lstRatePlans As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lblAccessCode.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblAccessCode As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtAccessCode.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtAccessCode As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control lblCD.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblCD As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtCD.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtCD As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control Label1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label1 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblRateApply.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRateApply As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control chkGDS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkGDS As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control chkPortal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkPortal As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control chkUnipantalla.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkUnipantalla As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control chkADS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkADS As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control lblOrden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblOrden As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddlOrden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlOrden As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control rvOrden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rvOrden As Global.System.Web.UI.WebControls.RangeValidator

    '''<summary>
    '''Control chkWaitListAvailable.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkWaitListAvailable As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control lblComision.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblComision As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblContratos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblContratos As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddlContratosNR.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlContratosNR As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control TextBoxTarifaComisionable.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents TextBoxTarifaComisionable As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control tipoPago.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tipoPago As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lblTipoPago.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTipoPago As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddlTipoPago.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlTipoPago As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control trComisiones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents trComisiones As Global.System.Web.UI.HtmlControls.HtmlTableCell

    '''<summary>
    '''Control trPorcGDS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents trPorcGDS As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''Control lblPorcGDS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblPorcGDS As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtPorcGDS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtPorcGDS As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control trPorcUni.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents trPorcUni As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''Control lblPorcUni.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblPorcUni As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtPorcUNI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtPorcUNI As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control trPorcPortal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents trPorcPortal As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''Control lblPorcPortal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblPorcPortal As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtPorcPOR.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtPorcPOR As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control trPorcADS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents trPorcADS As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''Control lblPorcADS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblPorcADS As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtPorcADS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtPorcADS As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control CheckBoxDeal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents CheckBoxDeal As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control tableConfDeal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tableConfDeal As Global.System.Web.UI.HtmlControls.HtmlTable

    '''<summary>
    '''Control lblFrom.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFrom As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtInicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtInicio As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control lblTo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtFinal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFinal As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control CheckBoxDefHora.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents CheckBoxDefHora As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control lblFromHora.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFromHora As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control HoraInicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents HoraInicio As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control MinutoInicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents MinutoInicio As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lblToHora.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblToHora As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control HoraFin.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents HoraFin As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control MinutoFin.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents MinutoFin As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control timevalidator.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents timevalidator As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control trGDSApply.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents trGDSApply As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''Control lblAplicaGDS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblAplicaGDS As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tdApplyGDS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tdApplyGDS As Global.System.Web.UI.HtmlControls.HtmlTableCell

    '''<summary>
    '''Control chkGDSAmadeus.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkGDSAmadeus As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control chkGDSGalileo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkGDSGalileo As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control chkGDSSabre.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkGDSSabre As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control chkGDSWorldSpan.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkGDSWorldSpan As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control lblPortal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblPortal As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblDeposittitle.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDeposittitle As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control RdbNone.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RdbNone As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control RdbOneNigth.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RdbOneNigth As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control rdbAlltotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdbAlltotal As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control lblPromotiontitle.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblPromotiontitle As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblGratis.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblGratis As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtDaysFree.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtDaysFree As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control lblFreeNights.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFreeNights As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblPromotion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblPromotion As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtDescProm.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtDescProm As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control lblDesc.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDesc As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtPromoDescription.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtPromoDescription As Global.RateManager.CtrlIdioma

    '''<summary>
    '''Control RVNochesgratis.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RVNochesgratis As Global.System.Web.UI.WebControls.RangeValidator

    '''<summary>
    '''Control RVPromotion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RVPromotion As Global.System.Web.UI.WebControls.RangeValidator
End Class
