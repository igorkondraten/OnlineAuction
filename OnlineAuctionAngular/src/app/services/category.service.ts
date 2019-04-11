import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  CATEGORY_LIST_PATH = 'http://localhost:54741/api/categories';

  constructor(private http: HttpClient) { }

  public getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(this.CATEGORY_LIST_PATH);
  }
}