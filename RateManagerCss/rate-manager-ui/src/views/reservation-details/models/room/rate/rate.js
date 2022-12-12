class rate {
    constructor(roomPriceId, checkIn, checkOut, price, extraPrice, priceNR, extraPriceNR,currency) {
        this.roomPriceId = roomPriceId;
        this.checkIn = checkIn;
        this.checkOut = checkOut;
        this.price = price;
        this.extraPrice = extraPrice;
        this.priceNR = priceNR;
        this.extraPriceNR = extraPriceNR,
        this.currency = currency;
    }
};

export default rate;
