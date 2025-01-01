import { Component, OnInit } from '@angular/core';
import { IProduct } from '../../Models/iproduct';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { ProductService } from '../../services/product.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule, FormsModule, NgxPaginationModule, HttpClientModule],
  providers: [ProductService],
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'] 
})
export class ProductComponent implements OnInit {
  products: IProduct = { entities: [], count: 0 }; 
  textSearch: string = '';
  resultOfSearch: IProduct[] | any; 
  pageItem: number = 7;
  pageNumber: number = 1;
  totalCount: number = 0;
  filteredProducts: any[] = [];
  filters = {
    minPrice: 0,
    maxPrice: 0,
  };

  constructor(
    private _productService: ProductService,
    private router: Router,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.getAll();
  }

  getAll() {
    this._productService.getAllProducts(this.pageItem, this.pageNumber).subscribe({
      next: (response: any) => { 
        this.products = response;
        console.log(this.products);
        this.totalCount = response.count;
        this.filteredProducts = [...this.products.entities]; // Initialize filtered products
      },
      error: (err) => {
        console.error(err); 
      }
    });
  }

  SearchByName() {
    this._productService.searchByName(this.textSearch, 10, 1).subscribe({
      next: (result: any) => {
        console.log(result);
        this.filteredProducts = result.entities;
        this.totalCount = result.count;
      },
      error: (err) => {
        console.error(err); 
      }
    });
  }

  deleteProduct(id: number) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '250px',
      data: 'Are you sure you want to delete this product?'
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === true) {
        this._productService.softDeleteProduct(id).subscribe({
          next: () => {
            this.router.navigateByUrl('/Products');
            this.getAll();
            this.showSuccessNotification();
          },
          error: (error) => {
            console.error(error);
            this.showErrorNotification();
          }
        });
      }
    });
  }

  onPageChange(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.getAll();
  }

  updateProduct(product: any) {
    this.router.navigateByUrl('/UpdateProduct', { state: { product } });
  }

  showSuccessNotification() {
    this.snackBar.open('Product deleted successfully!', 'Close', {
      duration: 3000, 
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: ['success-snackbar'] 
    });
  }

  showErrorNotification() {
    this.snackBar.open('Failed to delete the product.', 'Close', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: ['error-snackbar'] 
    });
  }

  applyFilters() {
    this.filteredProducts = [...this.products.entities]; // Reset to all products before applying filters
    this.applyPriceFilter();
  }

  applyPriceFilter() {
    if (this.filters.minPrice !== null && this.filters.minPrice !== undefined) {
      this.filteredProducts = this.filteredProducts.filter(product => product.price >= this.filters.minPrice);
    }
    if (this.filters.maxPrice !== null && this.filters.maxPrice !== undefined) {
      this.filteredProducts = this.filteredProducts.filter(product => product.price <= this.filters.maxPrice);
    }
  }
}
