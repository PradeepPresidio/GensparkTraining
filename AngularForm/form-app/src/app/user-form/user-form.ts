import { Component } from '@angular/core';
import {FormBuilder, FormGroup,Validators,ReactiveFormsModule} from '@angular/forms';
import { UserModel } from './user.model';
import { CustomValidators } from './user-form.validators';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-user-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-form.html',
  standalone : true,
  styleUrl: './user-form.css'
})
export class UserForm {
userForm: FormGroup;
roles: string[] = ['Admin','User',"Guest"];
blacklistedUsernames = ["admin","root"];
users: UserModel[] = [];
constructor(private fb:FormBuilder)
{
  this.userForm = this.fb.group({
    username:['',[Validators.required,Validators.minLength(5)]],
    email:['',Validators.required,Validators.email],
    role:['',Validators.required],
    //regex explanation : atleast 1 number , and 1 symbol
    password:['',[Validators.required,Validators.minLength(6), Validators.pattern(/^(?=.*[0-9])(?=.*[^A-Za-z0-9]).+$/)]],
    confirmPassword:['',[Validators.required]]
  },{Validators: [CustomValidators.passwordMatchValidator,CustomValidators.blacklistUsernameValidator(this.blacklistedUsernames)]})
}
 onSubmit(): void {
    if (this.userForm.valid) {
      const { username, email, role, password } = this.userForm.value;
      const newUser: UserModel = { username, email, role, password };
      this.users.push(newUser);
      console.log('Users:', this.users);
      this.userForm.reset();
    } else {
      this.userForm.markAllAsTouched();
    }
  }
}