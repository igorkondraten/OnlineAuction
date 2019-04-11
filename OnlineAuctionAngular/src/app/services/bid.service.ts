import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Bid } from '../models/bid.model';

@Injectable({
  providedIn: 'root'
})
export class BidService {
  BID_PATH = 'http://localhost:54741/api/';

  constructor(private http: HttpClient) { }

  public getByLot(lotId: number): Observable<Bid[]> {
    const path = `lots/${lotId}/bids`;
    return this.http.get<Bid[]>(this.BID_PATH + path);
  }

  public getByUser(userId: number): Observable<Bid[]> {
    const path = `users/${userId}/bids`;
    return this.http.get<Bid[]>(this.BID_PATH + path);
  }

  public create(lotId: number, bid: Bid): Observable<Bid> {
    const path = `lots/${lotId}/bids`;
    const body = { price: bid.price };
    return this.http.post<Bid>(this.BID_PATH + path, body);
  }

  public delete(bidId: number): Observable<Bid> {
    const path = `bids/${bidId}`;
    return this.http.delete<Bid>(this.BID_PATH + path);
  }
}
