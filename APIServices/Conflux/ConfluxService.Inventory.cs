using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Xml.Linq;
using APIServices.Conflux.Models.Inventory.Response;
using Portal.Hotel.Common.Data;
using Portal.General.Common.Data;
using Portal.General.Facade;
using Portal.Hotel.Facade;
using System.Net.Http;
using System.Text;
using System.Configuration;

namespace APIServices.Conflux
{
    public partial class ConfluxService
    {
        public InventoryResponse UpdateInventory(List<XDocument> documents, string endpoint)
        {
            InventoryResponse res = new InventoryResponse();

            var uri = new Uri(endpoint);

            try
            {
                foreach (XDocument document in documents)
                {
                    InventoryHttpResponse inventoryHttpResponse = new InventoryHttpResponse();

                    HttpContent httpContent = new StringContent(document.ToString(), Encoding.UTF8, "application/xml");

                    string result = string.Empty;

                    using (var client = new HttpClient())
                    {

                        client.Timeout = TimeSpan.FromMinutes(50);
                        var response = client.PostAsync(uri, httpContent).Result;
                        result = response.Content.ReadAsStringAsync().Result; //regresa un xml
                    }


                    inventoryHttpResponse.Xml = result;
                    inventoryHttpResponse.XmlRequest = document.ToString();
                    inventoryHttpResponse.IsSuccess = true;
                    res.InventoryHttpResponseList.Add(inventoryHttpResponse);

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                res.IsSuccess = true;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Error = new KeyValuePair<string, string>("448", ex.Message);
                var errorsElement = new System.Xml.Linq.XElement("Errors");
                var errorElementProperty = new System.Xml.Linq.XElement("Error");
                errorElementProperty.Add(new System.Xml.Linq.XAttribute("Type", "3"), new System.Xml.Linq.XAttribute("Code", "448"), new System.Xml.Linq.XText(ex.Message));
                errorsElement.Add(errorElementProperty);
                res.Xml = errorsElement.ToString();
            }

            return res;
        }

        public InventoryResponse UpdateInventoryPatch(List<XDocument> documents, string endpoint)
        {
            InventoryResponse res = new InventoryResponse();

            try
            {
                foreach (XDocument document in documents)
                {
                    InventoryHttpResponse inventoryHttpResponse = new InventoryHttpResponse();

                    HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint);
                    request.Content = new StringContent(document.ToString());

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["confluxApiUrl"].ToString());

                        var responseRequest = client.SendAsync(request).Result;

                        inventoryHttpResponse.Xml = responseRequest.Content.ReadAsStringAsync().Result;
                        inventoryHttpResponse.XmlRequest = document.ToString();
                        inventoryHttpResponse.IsSuccess = true;
                        res.InventoryHttpResponseList.Add(inventoryHttpResponse);

                    }

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                res.IsSuccess = true;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Error = new KeyValuePair<string, string>("448", ex.Message);
                var errorsElement = new System.Xml.Linq.XElement("Errors");
                var errorElementProperty = new System.Xml.Linq.XElement("Error");
                errorElementProperty.Add(new System.Xml.Linq.XAttribute("Type", "3"), new System.Xml.Linq.XAttribute("Code", "448"), new System.Xml.Linq.XText(ex.Message));
                errorsElement.Add(errorElementProperty);
                res.Xml = errorsElement.ToString();
            }

            return res;

        }

        public RoomsInventoryData GetInventoryData(int[] roomsIdList, DateTime? startDate, DateTime? endDate) 
        {
            RoomsInventoryData ds = new RoomsInventoryData();
            ds.Tables[0].Columns.Add("RoomCode", typeof(string));

            RoomsInventoryData dsActualInventory;

            for (int i = 0; i < roomsIdList.Length; i++)
            {
                 dsActualInventory = new RoomsInventoryFacade().getInventoryByDate_Data(roomsIdList[i], startDate.Value.Date, endDate.Value.Date);

                foreach (DataRow row in dsActualInventory.Tables[0].Rows)
                {
                    DataRow dr = ds.Tables[RoomsInventoryData.TBL_ROOMS_INVENTORY].NewRow();

                    dr[RoomsInventoryData.FLD_DATE] = row[RoomsInventoryData.FLD_DATE];
                    dr[RoomsInventoryData.FLD_STARTDATE] = row[RoomsInventoryData.FLD_DATE];
                    dr[RoomsInventoryData.FLD_ENDDATE] = row[RoomsInventoryData.FLD_DATE];
                    dr[RoomsInventoryData.FLD_ID_ROOM_HOTEL] = roomsIdList[i];
                    dr[RoomsInventoryData.FLD_NUMBER_ROOMS] = row[RoomsInventoryData.FLD_NUMBER_ROOMS];
                    dr[RoomsInventoryData.FLD_STATUS] = 0;
                    dr[RoomsInventoryData.FLD_NUMBER_AVAILABILITY] = row[RoomsInventoryData.FLD_NUMBER_AVAILABILITY];
                    dr["RoomCode"] = row["CodigoHabitacion"];

                    ds.Tables[RoomsInventoryData.TBL_ROOMS_INVENTORY].Rows.Add(dr);
                    dr.AcceptChanges();
                    dr[RoomsInventoryData.FLD_STATUS] = dr[RoomsInventoryData.FLD_STATUS];
                }
                
            }

            return ds;

        }

        public int[] LoadRoomsByIdHotel(int idHotel, int lang = 1)
        {
            List<int> roomsIdList = new List<int>();
            RoomsHotelData ds = new RoomFacade().getRooms(idHotel, lang);

            var roomsTable = ds.Tables[RoomsHotelData.TBL_ROOM_HOTEL];

            foreach (DataRow row in roomsTable.Rows)
            {
                string code = row.ItemArray[0].ToString();
                roomsIdList.Add(Convert.ToInt32(code));

            }

            return roomsIdList.ToArray();
        }

    }
}
