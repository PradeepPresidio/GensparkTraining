import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Products } from './products/products';
import { Cart } from './cart/cart';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet,Products,Cart],
  templateUrl: './app.html',
  styleUrl: './app.css',
  standalone : true
})
export class App {
  protected title = 'MYAPP';
}
