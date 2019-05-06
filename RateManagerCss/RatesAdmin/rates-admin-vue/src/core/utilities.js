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
    }
}