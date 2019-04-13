import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Bid } from '../models/bid.model';
import { catchError } from 'rxjs/operators';
import { ErrorHandlingService } from './error-handling.service';

@Injectable({
  providedIn: 'root'
})
export class BidService {
  BID_PATH = 'http://localhost:54741/api/';

  constructor(private http: HttpClient, private errorHandlingService: ErrorHandlingService) { }

  public getByLot(lotId: number): Observable<Bid[]> {
    const path = `lots/${lotId}/bids`;
    return this.http.get<Bid[]>(this.BID_PATH + path).pipe(
      catchError(this.errorHandlingService.handleError));
  }

  public getByUser(userId: number): Observable<Bid[]> {
    const path = `users/${userId}/bids`;
    return this.http.get<Bid[]>(this.BID_PATH + path).pipe(
      catchError(this.errorHandlingService.handleError));
  }

  public create(lotId: number, bid: Bid): Observable<Bid> {
    const path = `lots/${lotId}/bids`;
    const body = { price: bid.price };
    return this.http.post<Bid>(this.BID_PATH + path, body).pipe(
      catchError(this.errorHandlingService.handleError));
  }

  public delete(bidId: number): Observable<Bid> {
    const path = `bids/${bidId}`;
    return this.http.delete<Bid>(this.BID_PATH + path).pipe(
      catchError(this.errorHandlingService.handleError));
  }
}
