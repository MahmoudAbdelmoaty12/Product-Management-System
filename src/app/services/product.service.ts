import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IProduct } from '../Models/iproduct';
import { IProductCreate } from '../Models/iproduct-create';
import { IProductDetails } from '../Models/iproduct-details';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private baseUrl = 'http://localhost:5153/api/Product'; 

  constructor(private http: HttpClient) {}


  getAllProducts(
    pageItem: number = 10,
    pageNumber: number = 1,
    minPrice?: number,
    maxPrice?: number
  ): Observable<IProduct> {
    let params = new HttpParams()
      .set('pageItem', pageItem.toString())
      .set('pageNumber', pageNumber.toString());

   
    if (minPrice) {
      params = params.set('minPrice', minPrice.toString());
    }
    if (maxPrice) {
      params = params.set('maxPrice', maxPrice.toString());
    }

    return this.http.get<IProduct>(`${this.baseUrl}/GetAll`, { params });
  }


  createProduct(product: FormData): Observable<IProductCreate> {
    return this.http.post<IProductCreate>(this.baseUrl, product);
  }

  
  hardDeleteProduct(productId: number): Observable<IProductDetails> {
    return this.http.delete<IProductDetails>(`${this.baseUrl}/${productId}`);
  }

 
  softDeleteProduct(productId: number): Observable<IProductDetails> {
    const params = new HttpParams().set('ProductId', productId.toString());
    return this.http.delete<IProductDetails>(`${this.baseUrl}/SoftDeleteProduct`, { params });
  }


  updateProduct(product: FormData): Observable<IProductCreate> {
    return this.http.put<IProductCreate>(this.baseUrl, product);
  }


  searchByName(name: string, itemsPerPage: number, pageNumber: number): Observable<IProduct> {
    const params = new HttpParams()
      .set('Name', name)
      .set('ItemsPerPage', itemsPerPage.toString())
      .set('PageNumber', pageNumber.toString());
    return this.http.get<IProduct>(`${this.baseUrl}/SearchByName`, { params });
  }
 
  
  
}
