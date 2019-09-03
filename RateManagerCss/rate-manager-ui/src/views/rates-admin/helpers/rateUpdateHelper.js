import moment from 'moment';

const toNumber = (source) => {
    const val = Number(source);
    return !Number.isNaN(val) ? val : 0;
};

class RateUpdatHelper {
    constructor(room, ratePlan, dateRange, promotion,
        areOccupancyPrices, prices, overrideRules, rules) {
        this.errors = [];
        this.warnings = [];

        this.__$ = {
            room,
            ratePlan,
            dateRange,
            promotion,
            areOccupancyPrices,
            prices,
            overrideRules,
            rules,
        };
    }

    /**
     * Validación del request
     */
    validate() {
        // realizar validación;
        if (!this.__$.room?.id) {
            this.errors.push('room not selected');
        }

        if (!this.__$.ratePlan?.code) {
            this.errors.push('rate plan not selected');
        }
        // las dos primeras validaciones son obligatorias
        if (this.errors.length > 0) return;

        if (
            this.__$.promotion?.discount !== null
            && this.__$.promotion?.discount !== ''
            && this.__$.promotion?.discount !== undefined
        ) {
            if (
                !(toNumber(this.__$.promotion?.discount) > 0)
                || !(toNumber(this.__$.promotion?.discount) < 100)
            ) {
                this.errors.push('promo discount must greater than 0 and less than 100');
            }
            if (!this.__$.promotion.englishDescription) this.errors.push('english promo description not defined');
            if (!this.__$.promotion.spanishDescription) this.errors.push('spanish promo description not defined');
        } else {
            // si no hay descuento se ignora por completo
            this.__$.promotion = null;
        }

        if (!this.__$.areOccupancyPrices) {
            // tarifas for habitación
            if (toNumber(this.__$.prices?.byRoom?.adult) <= 0) {
                this.errors.push('adult rate must be greater than 0');
            }

            if (
                toNumber(this.__$.room?.maxChildrenOccupancy) > 0
                && toNumber(this.__$.prices.byRoom.child) <= 0
            ) {
                this.warnings.push('children rate is 0');
            }

            if (this.__$.room.juniorAllowed
                && toNumber(this.__$.room.maxChildrenOccupancy) > 0
                && toNumber(this.__$.prices?.byRoom?.junior) <= 0) {
                this.warnings.push('junior rate is 0');
            }
        } else {
            // tarifas for ocupación
            const prices = this.__$.prices.byOccupancy;

            if (prices.adult.some(rate => toNumber(rate.price) <= 0)) {
                this.errors.push('all adult rates must be greater than 0');
            }

            if (
                toNumber(this.__$.room?.maxChildrenOccupancy) > 0
                && prices.child.some(rate => toNumber(rate.price) <= 0)
            ) {
                this.warnings.push('some children rates are 0');
            }

            if (this.__$.room.juniorAllowed
                && toNumber(this.__$.room.maxChildrenOccupancy) > 0
                && prices.junior.some(rate => toNumber(rate.price) <= 0)) {
                this.warnings.push('some junior rates are 0');
            }


            // si hay dia de excepcion seleccionado
            if (
                this.__$.prices.exceptions
                && Object.keys(this.__$.prices.exceptions.apply).some(k => this.__$.prices.exceptions.apply[k])
            ) {
                const { exceptions } = this.__$.prices;
                if (exceptions.adult.some(rate => toNumber(rate.price) <= 0)) {
                    this.errors.push('exception adult rates must be greater than 0');
                }

                if (
                    toNumber(this.__$.room?.maxChildrenOccupancy) > 0
                    && exceptions.child.some(rate => toNumber(rate.price) <= 0)
                ) {
                    this.warnings.push('some exception children rates are 0');
                }

                if (this.__$.room.juniorAllowed
                    && toNumber(this.__$.room.maxChildrenOccupancy) > 0
                    && exceptions.some(rate => toNumber(rate.price) <= 0)) {
                    this.warnings.push('some exception junior rates are 0');
                }
            }
        }

        // precios de personas extra
        if (toNumber(this.__$.room.extraOccupancyAllowed) > 0) {
            if (toNumber(this.__$.prices?.extra?.adult) <= 0) {
                this.warnings.push('extra adult rate is 0');
            }

            if (
                toNumber(this.__$.room?.maxChildrenOccupancy) > 0
                && toNumber(this.__$.prices?.extra?.child) <= 0
            ) {
                this.warnings.push('extra child rate is 0');
            }

            if (this.__$.room.juniorAllowed
                && toNumber(this.__$.room.maxChildrenOccupancy) > 0
                && toNumber(this.__$.prices?.extra?.junior) <= 0) {
                this.warnings.push('extra junior rate is 0');
            }
        }

        try {
            if (this.__$.overrideRules) {
                const { rules } = this.__$;
                // sea a especificado ventana de reserva
                if (rules?.bookingWindow) {
                    if (!moment(rules?.bookingWindow?.end).isSameOrAfter(rules?.bookingWindow?.start)) {
                        this.errors.push('booking window end date must be after start date');
                    }
                }

                if (rules?.maxGuests !== null && rules?.maxGuests !== '') {
                    if (toNumber(rules?.maxGuests) <= 0) {
                        this.errors.push('max peole must be greater than 0');
                    }

                    const maxGuests = toNumber(rules?.maxGuests);

                    if (toNumber(rules?.children) > maxGuests) {
                        this.errors.push('children number cannot be greater than max peole');
                    }

                    if (toNumber(rules?.maxAdults) > maxGuests) {
                        this.errors.push('max adults cannot be greater than max peole');
                    }
                }


                if (rules?.maxAdults !== null && rules?.maxAdults !== '') {
                    if (toNumber(rules?.maxAdults) <= 0) {
                        this.errors.push('max adults must be greater than 0');
                    }
                }

                if (rules?.minAdults !== null && rules?.minAdults !== '') {
                    if (toNumber(rules?.minAdults) > toNumber(rules?.maxAdults)) {
                        this.errors.push('min adults cannot be greater than max adults');
                    }
                }

                if (
                    rules?.maxAdvanceBooking !== null
                    && rules?.maxAdvanceBooking !== ''
                    && toNumber(rules?.maxAdvanceBooking) > 0
                ) {
                    if (toNumber(rules?.minAdvanceBooking) > toNumber(rules?.maxAdvanceBooking)) {
                        this.errors.push('min advance booking cannot be greater than max advance booking');
                    }
                }

                if (rules?.maxLOS !== null && rules?.maxLOS !== '' && toNumber(rules?.maxLOS) > 0) {
                    if (toNumber(rules?.minLOS) > toNumber(rules?.maxLOS)) {
                        this.errors.push('min nights cannot be greater than max nights');
                    }
                }
            } else {
                // si la sobreescritura de reglas no esta seleccionada las eliminamos
                this.__$.rules = null;
            }
        } catch (error) {
            this.errors('invalid request, please contact support');
        }
    }

    createRQ() {
        if (this.errors?.length > 0) return null;

        let prices = null;

        if (this.__$.areOccupancyPrices) {
            prices = {
                exceptionDays: this.__$.prices.exceptions?.apply,
                base: this.__$.prices.byOccupancy.adult,
            };

            if (this.__$.room.maxChildrenOccupancy > 0) {
                prices.base = prices.base.concat(this.__$.prices.byOccupancy.child);
            }

            if (this.__$.room.maxChildrenOccupancy > 0 && this.__$.room.juniorsAllowed) {
                prices.base = prices.base.concat(this.__$.prices.byOccupancy.junior);
            }

            // si hay dia de excepcion seleccionado
            if (prices.exceptionDays && Object.keys(prices.exceptionDays).some(k => prices.exceptionDays[k])) {
                prices.exceptions = this.__$.prices.exceptions.adult;

                if (this.__$.room.maxChildrenOccupancy > 0) {
                    prices.exceptions = prices.exceptions.concat(this.__$.prices.exceptions.child);
                }

                if (this.__$.room.maxChildrenOccupancy > 0 && this.__$.room.juniorsAllowed) {
                    prices.exceptions = prices.exceptions.concat(this.__$.prices.exceptions.junior);
                }
            }
        } else {
            prices = {
                base: [
                    { occupation: 1, type: 1, price: this.__$.prices.byRoom.adult },
                ],
            };
            if (this.__$.room.maxChildrenOccupancy > 0) {
                prices.base.push({ occupation: 1, type: 2, price: this.__$.prices.byRoom.child });
            }
            if (this.__$.room.maxChildrenOccupancy > 0 && this.__$.room.juniorsAllowed) {
                prices.base.push({ occupation: 1, type: 3, price: this.__$.prices.byRoom.junior });
            }
        }

        if (this.__$.room.extraOccupancyAllowed > 0) {
            prices.extra = [
                { occupation: 1, type: 1, price: this.__$.prices.extra.adult },
            ];
            if (this.__$.room.maxChildrenOccupancy > 0) {
                prices.extra.push({ occupation: 1, type: 2, price: this.__$.prices.extra.child });
            }
            if (this.__$.room.maxChildrenOccupancy > 0 && this.__$.room.juniorsAllowed) {
                prices.extra.push({ occupation: 1, type: 3, price: this.__$.prices.extra.junior });
            }
        }

        if (this.__$.promotion) prices.promotion = this.__$.promotion;

        const RQ = {
            roomId: this.__$.room?.id,
            ratePlanCode: this.__$.ratePlan?.code,
            startDate: moment(this.__$.dateRange?.start).format('YYYY-MM-DD'),
            endDate: moment(this.__$.dateRange?.end).format('YYYY-MM-DD'),
            isOccupancyRate: this.__$.areOccupancyPrices,
            prices,
        };

        if (this.__$.overrideRules) {
            const { rules } = this.__$;

            const rulesRQ = {
                noArrival: rules.noArrival,
            };

            if (toNumber(rules.minLOS) > 0) rulesRQ.minLOS = toNumber(rules.minLOS);
            if (toNumber(rules.maxLOS) > 0) rulesRQ.maxLOS = toNumber(rules.maxLOS);
            if (toNumber(rules.minAdvanceBooking) > 0) rulesRQ.minAdvanceBooking = toNumber(rules.minAdvanceBooking);
            if (toNumber(rules.maxAdvanceBooking) > 0) rulesRQ.maxAdvanceBooking = toNumber(rules.maxAdvanceBooking);

            if (rules?.bookingWindow) {
                rulesRQ.bookingWindow = {
                    startDate: moment(rules.bookingWindow.start).format('YYYY-MM-DD'),
                    endDate: moment(rules.bookingWindow.end).format('YYYY-MM-DD'),
                };
            }

            RQ.rules = rulesRQ;
        }

        return RQ;
    }
}

export default RateUpdatHelper;
