using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrinityApi.Core.Entities
{
    public static class Events
    {
        public const string
            SESSION = "SESSION",
            SUBSCRIBE = "SUBSCRIBE",
            SUBSCRIBE_ALL = "ALL",
            SubscribeLastConfigId = "EVT_NEW_CONFIG_APPLIED",
            PumpStatusChangeId = "EVT_PUMP_STATUS_CHANGE_ID_*",
            NewConfigApplied = "EVT_NEW_CONFIG_APPLIED",
            PumpDeliveryProgress = "EVT_PUMP_DELIVERY_PROGRESS_ID_",
            PumpTotalizerUpdateId = "EVT_PUMP_TOTALIZER_UPDATE_ID_",
            PumpNewTransaction = "EVT_PUMP_NEW_TRANSACTION",
            PumpPriceLevelId = "EVT_PUMP_PRICE_LEVEL_ID_*",
            GradePriceChange = "EVT_GRADE_PRICE_CHANGE",
            EntryPumpController = "EVT_ENTRY_PUMP_CONTROLLER",
            ExitPumpController = "EVT_EXIT_PUMP_CONTROLLER",
            EntryClientConnection = "EVT_ENTRY_CLIENT_CONNECTION",
            ExitClientConnection = "EVT_EXIT_CLIENT_CONNECTION",
            ShiftPeriodClosed = "EVT_SHIFT_PERIOD_CLOSED",
            TktPeriodClosed = "EVT_TKT_PERIOD_CLOSED_",
            PaymentSaleCleared = "EVT_PAYMENT_SALE_CLEARED",
            PaymentSaleWarningOn = "EVT_PAYMENT_SALE_WARNING_ON",
            PaymentSaleWarningOff = "EVT_PAYMENT_SALE_WARNING_OFF",
            PaymentPumpWarningOff = "EVT_PAYMENT_SALE_WARNING_OFF",
            PaymentPumpWarningON = "EVT_PAYMENT_PUMP_WARNING_ON",
            PaymentAtributesChange = "EVT_PAYMENT_ATTRIBUTES_CHANGE",
            PumpErrorMsgId = "EVT_PUMP_ERROR_MSG_ID_",
            TktTrxNewId = "EVT_TKT_TRX_NEW_ID_",
            TktTrxUpdateId = "EVT_TKT_TRX_UPDATE_ID_",
            TktPeriodClose = "EVT_TKT_PERIOD_CLOSE";
    }
}

