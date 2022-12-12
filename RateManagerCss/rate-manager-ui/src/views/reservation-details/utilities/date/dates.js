export function sortDatesInPlace(dates) {

    dates.sort((previous, current) => {  
        // get the start date from previous and current
        var previousTime = previous.start.getTime();
        var currentTime = current.start.getTime();

        // if the previous is earlier than the current
        if (previousTime < currentTime) {
        return -1;
        }

        // if the previous time is the same as the current time
        if (previousTime === currentTime) {
        return 0;
        }

        // if the previous time is later than the current time
        return 1;
    });
    
    return dates;

};

export function overlapDates(dates) {
    let localDates = Object.create(dates);
    var sortedRanges = sortDatesInPlace(localDates);
    var result = overlap(sortedRanges);

    return result;
};

function overlap (dates) {

    return ( dates.reduce((result, current, idx, arr) => {
        // get the previous range
        if (idx === 0) { return result; }
        var previous = arr[idx-1];
    
        // check for any overlap
        var previousEnd = previous.end.getTime();
        var currentStart = current.start.getTime();
        var overlap = (previousEnd >= currentStart);
    
        // store the result
        if (overlap) {
            // yes, there is overlap
            result.isOverlap = true;
            // store the specific ranges that overlap
            result.ranges.push({
                previous: previous,
                current: current
            })
        }
    
        return result;
    
        // seed the reduce  
    }, {isOverlap: false, ranges: []} ) );

}

