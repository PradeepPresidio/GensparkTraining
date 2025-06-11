import { Component,ViewChild } from '@angular/core';
import { Products } from '../products/products';
@Component({
  selector: 'app-cart',
  imports: [Products],
  templateUrl: './cart.html',
  styleUrl: './cart.css',
  standalone : true
})
export class Cart {
  @ViewChild(Products) products!: Products;
  counter : number = 0;
  ngAfterViewInit()
  {
    setTimeout(() => {
    if (this.products) {
      this.counter = this.products.counter;
    }
  });
  }
}
