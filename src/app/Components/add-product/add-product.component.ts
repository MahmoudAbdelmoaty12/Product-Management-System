import { Component, OnInit } from '@angular/core';
import { IProductCreate } from '../../Models/iproduct-create';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

  formData: FormData = new FormData();
  newProduct: IProductCreate = {
    Id: 0,
    Name: '',
    Price: 0,
    Description: '',
    Images: [],
  };

  constructor(
    private _productService: ProductService,
    private router: Router
  ) { }

  ngOnInit(): void { }

  addProduct() {
   
    if (this.newProduct.Images.length === 0) {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'At least one image must be selected!',
      });
      return;
    }

  
    this.formData.append('Description', this.newProduct.Description);
    this.formData.append('Name', this.newProduct.Name);
    this.formData.append('Price', this.newProduct.Price.toString());

    this._productService.createProduct(this.formData).subscribe({
      next: (response) => {
        if (response) {
          Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Product added successfully!',
          });
          this.router.navigateByUrl('/Products');
        } else {
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Product Already Exists',
          });
        }
       
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'An error occurred while adding the product.',
        });
        console.log('Error:', error);
       
      }
    });
  }

  onImageSelected(event: any) {
    const files = (event.target as HTMLInputElement).files;
    if (files && files.length > 0) {
      this.newProduct.Images = [];
      for (let i = 0; i < files.length; i++) {
        const file = files[i];   
          this.formData.append('Images', file, file.name);
          this.newProduct.Images.push(file)
      }
    }
  }
}

