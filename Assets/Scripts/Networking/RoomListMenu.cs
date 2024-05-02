using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform roomListingContent;
    [SerializeField] private RoomListing roomListingPrefab;

    private List<RoomListing> listings = new List<RoomListing>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(listings[index].gameObject);
                    listings.RemoveAt(index);
                }
            }
            else
            {
                RoomListing listing = Instantiate(roomListingPrefab, roomListingContent);
                if (listing != null)
                {
                    listing.SetRoomInfo(info);
                    listings.Add(listing);
                }
            }
        }
    }
}
