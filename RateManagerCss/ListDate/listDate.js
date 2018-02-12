function isDate(year, month, day) {
	month = month - 1;
	var tempDate = new Date(year,month,day);
	if ( (tempDate.getFullYear() == year) && (month == tempDate.getMonth()) && (day == tempDate.getDate()) )
		return true;
	else
		return false
}

function renderDay(idDropDownDays,idDropDownMonth,idDropDownYear)
{
	var lstDay=document.getElementById(idDropDownDays);
	var lstMonth=document.getElementById(idDropDownMonth);
	var lstYear=document.getElementById(idDropDownYear);
	var indexDay = lstDay.selectedIndex;
	if(indexDay<0) indexDay=0;
	
	/*Clear the list*/
     while (lstDay.options.length>0){
		deleteIndex=lstDay.options.length-1;
		lstDay.options[deleteIndex]=null;
	}

	var month = lstMonth.options[lstMonth.selectedIndex].value;
	var year = lstYear.options[lstYear.selectedIndex].value;
	var insertIndex = 0;
	
	/*Load days by month*/
	for(i=1;i<=31;i++){				
		if(isDate(year,month,i)){
			newOption = new Option();
			newOption.text=i
			newOption.value=i
			insertIndex=lstDay.options.length;
			lstDay.options[insertIndex]=newOption;	  
		}				
	}	
	if(lstDay.options.length>indexDay){	
		lstDay.selectedIndex=indexDay;		
	}
	else /* Last item selected */
		lstDay.selectedIndex=insertIndex;
}
