import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { IProduct } from './Models/iproduct';
import { ProductService } from './services/product.service';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from './Components/sidebar/sidebar.component';
import { HeaderComponent } from './Components/header/header.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,CommonModule
    ,SidebarComponent , HeaderComponent ,FormsModule,
    HttpClientModule, 
  ],
  providers: [ProductService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  products: IProduct['entities'] = [];
  totalProducts = 0;
  isLoading = true;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getAllProducts(10, 1).subscribe({
      next: (data) => {
        this.products = data.entities;
        this.totalProducts = data.count;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching products:', err);
        this.isLoading = false;
      },
    });
  }
}
