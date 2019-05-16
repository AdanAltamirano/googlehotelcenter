import moment from 'moment';

export default {
    /**
     * @returns {moment} moment date object
     */
    getLastWorkDay(){
        let today = moment();
        if(localStorage){   
            let _lastWorkDay = localStorage.getItem('admin.rates.workingDate');
            if(_lastWorkDay){
                let start = moment(_lastWorkDay);               
                if(start.isValid() && start.isBefore(today)){
                    return start;
                }             
            }
            localStorage.setItem('admin.rates.workingDate', today.format('YYYY-MM-DD'));
        }
        return today;
    },
    /**
     * 
     * @param {[Any]} array rateplan array 
     */
    makeRatesTree(array){
        var map = {}, node, roots = [], i;

        for(i = 0; i < array.length; i++){
            map[array[i].ratePlanId + array[i].roomId] = i;
            array[i].children = [];
        }

        for(i = 0; i < array.length; i++){
            node = array[i];
            if(node.parentRatePlanId){
                array[map[node.parentRatePlanId + node.roomId]].children.push(node);
            }
            else{
                roots.push(node);
            }
        }
        return  roots;
    },
    
    /**
     * 
     * @param {[Any]} rooms rooms array 
     * @param {[Any]} rates rateplan array 
     */
    mixRoomsAndRates(rooms, rates) {
        let ratesTree = this.makeRatesTree(rates);
        return rooms.map((room) => {
            room.rates = ratesTree.filter(plan => plan.roomId == room.id);
            return room;
        })
        .filter(room => room.rates && room.rates.length > 0);
    }
}