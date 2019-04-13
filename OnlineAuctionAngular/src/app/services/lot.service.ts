import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Lot } from '../models/lot.model';
import { LotList } from '../models/lot-list.model';
import { catchError } from 'rxjs/operators';
import { ErrorHandlingService } from './error-handling.service';

@Injectable({
  providedIn: 'root'
})
export class LotService {
  LOT_LIST_PATH = 'http://localhost:54741/api/lots';  

  constructor(private http: HttpClient, private errorHandlingService: ErrorHandlingService) { }

  public getAll(limit: number, offset: number, query: string = null): Observable<LotList> {
    let pages_path = `/?limit=${limit}&offset=${offset}`;
    if (query)
      pages_path += `&query=${query}`;
    return this.http.get<LotList>(this.LOT_LIST_PATH + pages_path).pipe(
      catchError(this.errorHandlingService.handleError));
  }

  public get(lotId: number): Observable<Lot> {
    const path = `/${lotId}`;
    return this.http.get<Lot>(this.LOT_LIST_PATH + path).pipe(
      catchError(this.errorHandlingService.handleError));
  }

  public create(lot: Lot): Observable<Lot> {
    const body = {
      name: lot.name, 
      description: lot.description, 
      initialPrice: lot.initialPrice, 
      beginDate: lot.beginDate, 
      endDate: lot.endDate, 
      category: lot.category,
      image: lot.image };
    return this.http.post<Lot>(this.LOT_LIST_PATH, body).pipe(
      catchError(this.errorHandlingService.handleError));
  }

  public delete(lotId: number): Observable<{}> {
    const path = `/${lotId}`;
    return this.http.delete(this.LOT_LIST_PATH + path).pipe(
      catchError(this.errorHandlingService.handleError));
  }

  public update(lotId: number, lot: Lot): Observable<{}> {
    return this.http.put(this.LOT_LIST_PATH + `/${lotId}`, lot).pipe(
      catchError(this.errorHandlingService.handleError));
  }
}