import { Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { ProductComponent } from './Components/product/product.component';
import { AddProductComponent } from './Components/add-product/add-product.component';
import { UpdateProductComponent } from './Components/update-product/update-product.component';

export const routes: Routes = [
    {path:'' , redirectTo:'/Home' ,pathMatch:'full'},
    { path:'Home' , component:HomeComponent } ,
    { path:'Products' , component:ProductComponent} ,
    { path:'AddProduct' , component:AddProductComponent} ,   
    { path:'UpdateProduct' , component:UpdateProductComponent} ,








];
