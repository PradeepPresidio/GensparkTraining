import { Component } from '@angular/core';

@Component({
  selector: 'app-products',
  templateUrl: './products.html',
  styleUrls: ['./products.css'],
  standalone : true
})
export class Products {
  counter: number = 0;

  increment(): void {
    console.log("Hi");
    this.counter++;
  }

  decrement(): void {
    this.counter--;
  }

  reset(): void {
    this.counter = 0;
  }
}
