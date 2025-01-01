import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { IProductCreate } from '../../Models/iproduct-create';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-update-product',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './update-product.component.html',
  styleUrls: ['./update-product.component.css']
})
export class UpdateProductComponent {
  productId: number = 0;
  formData: FormData = new FormData();
  updateForm: FormGroup; // Define the form group

  constructor(
    private fb: FormBuilder,
    private _productService: ProductService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {
    this.updateForm = this.fb.group({
      Name: ['', [Validators.required]],
      Description: ['', [Validators.required]],
      Price: ['', [Validators.required, Validators.min(1)]],
      Images: [[], [Validators.required]],
    });
  }

  ngOnInit() {
    // Extract product data from route state or API
    const product = history.state.product;
    console.log('Product:', product);  // Ensure this logs correctly

    if (product && product.id) {
      this.productId = product.id;
      // Pre-fill the form with product data
      this.updateForm.patchValue({
        Name: product.name,
        Description: product.description,
        Price: product.price,
      });
    } else {
      console.error('Product ID is missing!');
    }
  }

  updateProduct() {
    if (this.updateForm.invalid) {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'Please fill in all required fields.',
      });
      return;
    }

    // Prepare the form data to submit
    this.formData.append('Id', this.productId.toString());
    this.formData.append('Name', this.updateForm.get('Name')?.value);
    this.formData.append('Description', this.updateForm.get('Description')?.value);
    this.formData.append('Price', this.updateForm.get('Price')?.value.toString());

    // Append selected images
    const images = this.updateForm.get('Images')?.value;
    if (images && images.length > 0) {
      for (let i = 0; i < images.length; i++) {
        this.formData.append('Images', images[i], images[i].name);
      }
    }

    this._productService.updateProduct(this.formData).subscribe({
      next: (response) => {
        if (response) {
          Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Product updated successfully!',
          });
          this.router.navigateByUrl('/Products');
        } else {
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Product update failed.',
          });
        }
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'An error occurred while updating the product.',
        });
        console.log('Error:', error);
      },
    });
  }

  onImageSelected(event: any) {
    const files = (event.target as HTMLInputElement).files;
    if (files && files.length > 0) {
      this.updateForm.patchValue({ Images: files });
    }
  }
}
