import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UserForm } from './user-form/user-form';
@Component({
  selector: 'app-root',
  standalone :true,
  imports: [RouterOutlet,UserForm],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'form-app';
}
