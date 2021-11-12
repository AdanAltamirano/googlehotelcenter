import Vue from 'vue';
import moment from 'moment';

export default {
    /**
     * Obtiene el último dia seleccionado,
     * si localstorage esta disponible lo obtiene de ahí
     * caso contrario devulve el día actual
     * @returns {moment} moment date object
     */
    getLastWorkDay() {
        const today = moment();
        if (localStorage) {
            const lastWorkDay = localStorage.getItem(`admin.${Vue.appConfig.session.hotelId}.rates.workingDate`);
            if (lastWorkDay) {
                const start = moment(lastWorkDay);
                if (start.isValid() && start.isAfter(today)) {
                    return start;
                }
            }
            this.setLastWorkDay(today);
        }
        return today;
    },
    /**
     * Guarda el último dia seleccionado en el localstorage
     * @param {moment} day
     */
    setLastWorkDay(day) {
        if (localStorage) {
            localStorage.setItem(`admin.${Vue.appConfig.session.hotelId}.rates.workingDate`, day.format('YYYY-MM-DD'));
        }
    },
    /**
     * Genera el arbol de rateplans, considerando que unicamente promociones pueden ser rateplan hijos
     * @param {[Any]} array rateplan array tree...
     */
    makeRatesTree(array) {
        const map = {}; let node; const roots = []; let
            i;

        for (i = 0; i < array.length; i += 1) {
            const item = array[i];
            map[item.ratePlanId + item.roomId] = i;
            item.children = [];
        }

        for (i = 0; i < array.length; i += 1) {
            node = array[i];
            if (node.parentRatePlanId && node.isPromotion) {
                if(map[node.parentRatePlanId + node.roomId] != undefined)
                    array[map[node.parentRatePlanId + node.roomId]].children.push(node);
            } else {
                roots.push(node);
            }
        }
        return roots;
    },

    /**
     * Mezcla de habitaciones con sus tarifas
     * @param {[Any]} rooms rooms array
     * @param {[Any]} rates rateplan array
     */
    mixRoomsAndRates(rooms, rates) {
        const ratesTree = this.makeRatesTree(rates).sort(a => (a.parentRatePlanId ? 1 : -1));
        return rooms.map((room) => {
            room.rates = ratesTree.filter(plan => plan.roomId === room.id);
            return room;
        }).filter(room => room.rates && room.rates.length > 0);
    },

    getFinalPrice(dayRate, rateInfo, room) {
        const { discount, discountLevel } = rateInfo;
        let { price } = dayRate;
        const dayDiscount = dayRate.discount;

        // primero se aplica la regla del linkeo
        if (rateInfo.factor !== undefined) price *= rateInfo.factor;
        else if (rateInfo.offset !== undefined) price += rateInfo.offset;

        // luego se verifica el nivel del decuento
        // se tiene el descuento de tarifa primero que es el mayor jerarquía
        // si es 0 se cambia por el del rateplan
        let finalDiscount = 0;
        if (discountLevel === 0) finalDiscount = dayDiscount || discount;
        // suma de descuentos
        else if (discountLevel === 1) finalDiscount = dayDiscount + discount;
        // descuento adicional
        else if (discountLevel === 2) {
            // adicionar 2 descuentos
            if (discount && dayDiscount) finalDiscount = 100 - ((100 - discount) * (100 - dayDiscount) / 100);
            // descuento adicional solo hay descuento rateplan
            else if (discount > 0) finalDiscount = discount;
            // descuento adicional solo hay descuento diario
            else if (dayDiscount > 0) finalDiscount = dayDiscount;
        }

        // aplicar descuento final
        price *= (1 - (finalDiscount / 100));

        // aplicar linkeo de habitación en caso de tenerlo.
        if (room?.isLiked) {
            if (room.factor !== undefined) price *= room.factor;
            else if (room.offset !== undefined) price += room.offset;
        }

        return price;
    },
};
