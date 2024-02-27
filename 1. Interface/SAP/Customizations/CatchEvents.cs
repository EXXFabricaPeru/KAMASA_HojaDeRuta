using SAPbouiCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Interface.Views;
using Exxis.Addon.HojadeRutaAGuia.Interface.Views.FormattedSearches;
//using Exxis.Addon.HojadeRutaAGuia.Interface.Views.Modal;
//using Exxis.Addon.HojadeRutaAGuia.Interface.Views.Modal.LiquidationTransporter;
using Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserObjectViews;
//using Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserViews;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Customizations
{
    public static class CatchEvents
    {
        public static void OnClickMenu(MenuEvent menuEvent, out bool handle)
        {
            handle = true;
            //if (menuEvent.MenuUID == TransferOrderValidating.APPROVE_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    var form = new TransferOrderValidate();
            //    form.Show();
            //}
            //else if (menuEvent.MenuUID == TransferOrderValidating.VIEW_DETAIL_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    var form = new TransferOrderValidate();
            //    form.ChangeToViewMode();
            //    form.Show();
            //}
            //else if (menuEvent.MenuUID == DocumentValidating.APPROVE_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    var form = new DocumentValidate();
            //    form.Show();
            //}
            //else if (menuEvent.MenuUID == OpeningTransferOrder.OPEN_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FROM_ORTR_OPENING.MakeOpenModal(OpenTransferOrder.FormTypeAction.ChangeStatus);
            //}
            //else if (menuEvent.MenuUID == OpeningTransferOrder.UPDATE_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    if (LocalStorage.FORM_TRANSFER_ORDER_OPENING.STAD != "PS")
            //        LocalStorage.FROM_ORTR_OPENING.MakeOpenModal(OpenTransferOrder.FormTypeAction.UpdateQuantity);
            //    else
            //        ApplicationInterfaceHelper.ShowStatusBarMessage("Solo se puede modificar OTs abiertas", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            //}
            //else if (menuEvent.MenuUID == ExceptionalUpdatingTransferOrder.UPDATE_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    var form = new ExceptionalUpdateTransferOrder();
            //    form.Show();
            //}
            //else if (menuEvent.MenuUID == TransferOrderValidate.INCLUDE_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    int documentEntry = LocalStorage.STORE_ORTR_DOCUMENT_INCLUDE;
            //    LocalStorage.FORM_ORTR_DOCUMENT_INCLUDE.ChangeInclusionStatus(documentEntry);
            //}
            //else if (menuEvent.MenuUID == ClearanceRoute.UPDATE_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    if (LocalStorage.STORE_OARD_CLS_ROTE.RouteStatus == OARD.Status.IN_PROGRESS)
            //    {
            //        var form = new UpdateClosingRoute();
            //        form.Show();
            //    }
            //    else
            //    {
            //        ApplicationInterfaceHelper.ShowStatusBarMessage("Solo se puede liquidar rutas en ejecución", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            //    }

            //}
            //else if (menuEvent.MenuUID == ClearanceRoute.CLOSING_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_ROTE.ClosingSelectedRoutes();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_ALL_TO_DELIVERED_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeAllToDelivered();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_TO_REJECTED_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeToRejected();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_TO_DELIVERED_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeToDelivered();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_TO_NOT_DELIVERED_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeToNotDelivered();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_TO_RESCHEDULED_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeToRescheduled();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_TO_RESCHEDULED_TRANS_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeToRescheduledTrans();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_ALL_TO_RETURNED_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeAllToReturned();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_TO_RETURNED_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeToReturned();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_TO_RETURNED_OBSERVATION_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeToReturnedWithObservation();
            //}
            //if (menuEvent.MenuUID == UpdateClosingRouteModal3.CHANGE_BATCH_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    var modal = new UpdateClosingRouteModal4();
            //    modal.SetItemCode(UpdateClosingRouteModal3.INSTANCE.ItemCode);
            //    modal.OnSelectBatch += UpdateClosingRouteModal3.INSTANCE.HandleSelectBatchFromModal;
            //    modal.Show();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_ALL_TO_TRANSFERRED_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeAllToTransferred();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_TO_TRANSFERRED_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeToTransferred();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.CHANGE_TO_TRANSFERRED_OBSERVATION_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.ChangeToTransferredWithObservation();
            //}
            //else if (menuEvent.MenuUID == UpdateClosingRoute.ASSIGN_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_CLS_RT_2.AssignBatches();
            //}
            //else if (menuEvent.MenuUID == FileUpload.SELECT_ALL_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_FILE_UPLOAD.SelectAll();
            //}
            //else if (menuEvent.MenuUID == FileUpload.UNSELECT_ALL_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_FILE_UPLOAD.UnSelectAll();
            //}
            //else if (menuEvent.MenuUID == FileUpload.UNSELECT_ALL_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_FILE_UPLOAD.UnSelectAll();
            //}
            //else if (menuEvent.MenuUID == RouteForm.MANUAL_EDIT_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.FORM_ROUTE_UDO.OpenManualEdit();
            //}
            //else if (menuEvent.MenuUID == RouteFormModal.REMOVE_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LocalStorage.MODAL_ROUTE_UDO.RemoveSelectRow();
            //}
            //else if (menuEvent.MenuUID == DocumentValidating.OFFHAND_APPROVE_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    DocumentValidating.INSTANCE.ApproveMarketingDocuments();
            //}
            //else if (menuEvent.MenuUID == DocumentValidating.OFFHAND_DISAPPROVE_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    DocumentValidating.INSTANCE.DisapproveMarketingDocuments();
            //}
            //else if (menuEvent.MenuUID == LiquidationTransporterForm.DETAIL_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LiquidationTransporterForm.Instance.ShowRouteDetailModal();
            //}
            //else if (menuEvent.MenuUID == LiquidationTransporterForm.CONFIRM_MENU_NAME && menuEvent.BeforeAction == false)
            //{
            //    LiquidationTransporterForm.Instance.ChangeRouteStatus();
            //}
            //else if (menuEvent.MenuUID == RouteDetailModal.REMOVE_MENU_NAME && menuEvent.BeforeAction == false && RouteDetailModal.HasInstanced)
            //{
            //    RouteDetailModal.Instance.RemoveSelectedMotive();
            //}
            //else if (menuEvent.MenuUID == LiquidationTransporterForm.REMOVE_MENU_ID && menuEvent.BeforeAction)
            //{
            //    handle = LiquidationTransporterForm.Instance.IsValidToRemoveRow();
            //}

            //else if(menuEvent.MenuUID == ApplicationInterfaceHelper.ADD_RECORD_MENU_ID && ApplicationInterfaceHelper.FormIsActive(LiquidationTransporterForm.SAP_FORM_TYPE))
            //{
            //    LiquidationTransporterForm.Instance.ClearFormToAddMode();
            //}
            //else if (menuEvent.MenuUID== ApplicationInterfaceHelper.CANCEL_MENU_ID&& menuEvent.BeforeAction == false)
            //{
            //    if (ApplicationInterfaceHelper.FormIsActive(TransferOrderForm.SAP_FORM_TYPE))
            //    {
            //        TransferOrderForm.RestoreQuantityOVFR();
            //    }
            //}
            //else if(menuEvent.MenuUID == ApplicationInterfaceHelper.CANCEL_MENU_ID && menuEvent.BeforeAction == true)
            //{
            //    if (ApplicationInterfaceHelper.FormIsActive(SaleOrderForm.SAP_FORM_TYPE))
            //    {
            //        SaleOrderForm.isCancel = true;
            //    }
            //}

            if (menuEvent.MenuUID == "VS_PD_OARD_Remove_Line" && menuEvent.BeforeAction == true)
            {
                ApplicationInterfaceHelper.ShowStatusBarMessage("No se puede eliminar líneas a la ruta", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);
                handle = false;
            }
            else if (menuEvent.MenuUID == "VS_PD_OARD_Add_Line" && menuEvent.BeforeAction == true)
            {
                ApplicationInterfaceHelper.ShowStatusBarMessage("No se puede añadir líneas a la ruta", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);
                handle = false;
            }

        }

        public static void OnGeneralApplicationEvents(string formId, ItemEvent itemEvent, out bool handle)
        {
            handle = true;

            //if (itemEvent.FormTypeEx == UploadEDITemplateForm.SAP_FORM_TYPE && itemEvent.Row > 1)
            //{

            //    ApplicationInterfaceHelper.ShowSuccessStatusBarMessage(
            //        $"itemEvent.BeforeAction={itemEvent.BeforeAction} itemEvent.EventType={itemEvent.EventType} itemEvent.ItemUID={itemEvent.ItemUID} itemEvent.ColUID={itemEvent.ColUID} itemEvent.Row={itemEvent.Row} itemEvent.InnerEvent={itemEvent.InnerEvent} itemEvent.FormType={itemEvent.FormType}");
            //}

            //if (itemEvent.FormTypeEx == UpdateClosingRoute.SAP_FORM_ID && itemEvent.BeforeAction && UpdateClosingRoute.ActivateModal &&
            //    itemEvent.EventType != BoEventTypes.et_VALIDATE)
            //{
            //    handle = false;
            //    UpdateClosingRoute.ShowModal();
            //}

            ////if (itemEvent.FormTypeEx == BuscaBatchDateOpenFormattedSearch.SAP_TYPE)
            //{
            //    if (itemEvent.EventType == BoEventTypes.et_FORM_CLOSE)
            //    {
            //        string selectedItemName = BuscaBatchDateOpenFormattedSearch.Instance.SelectedItemName;
            //        if (!string.IsNullOrEmpty(selectedItemName))
            //            OpeningTransferOrder.Instance.SetItemName(selectedItemName);
            //    }
            //}

            //if (itemEvent.FormTypeEx == OpeningTransferOrder.SAP_FORM_ID && itemEvent.BeforeAction && OpeningTransferOrder.ActivateModal &&
            //    itemEvent.EventType != BoEventTypes.et_VALIDATE)
            //{
            //    handle = false;
            //    OpeningTransferOrder.ShowModal();
            //}

            //if (itemEvent.FormTypeEx == TransporterForm.SAP_FORM_ID && itemEvent.BeforeAction && TransporterForm.ActivateModal &&
            //    itemEvent.EventType != BoEventTypes.et_VALIDATE)
            //{
            //    handle = false;
            //    TransporterForm.ShowModal();
            //}

            //if (itemEvent.FormTypeEx == RouteForm.SAP_FORM_TYPE && itemEvent.BeforeAction && (LocalStorage.FORM_ROUTE_UDO?.HasActiveModal ?? false) &&
            //    itemEvent.EventType != BoEventTypes.et_VALIDATE)
            //{
            //    handle = false;
            //    LocalStorage.FORM_ROUTE_UDO.FocusActiveModal();
            //}

            //if (itemEvent.FormTypeEx == LiquidationTransporterForm.SAP_FORM_TYPE && itemEvent.BeforeAction &&
            //    LiquidationTransporterForm.HasInstanced && LiquidationTransporterForm.Instance.HasActivateModals)
            //{
            //    handle = false;
            //    LiquidationTransporterForm.Instance.ShowModal();
            //}

            //if (itemEvent.FormTypeEx == ShipmentTransferForm.SAP_FORM_TYPE && itemEvent.BeforeAction &&
            //    ShipmentTransferForm.HasInstanced && ShipmentTransferForm.Instance.HasActivateModals)
            //{
            //    handle = false;
            //    ShipmentTransferForm.Instance.ShowActiveModal();
            //}


            //if (itemEvent.FormTypeEx == FileUpload.SAP_FORM_ID)
            //{
            //    handle = true;
            //    if (FileUpload.FinallyAddDataProcess)
            //    {
            //        FileUpload.ShowLastAddedRecord();
            //        handle = false;
            //    }
            //}
        }
    }
}