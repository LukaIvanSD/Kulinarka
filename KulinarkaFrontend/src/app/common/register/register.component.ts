import { CommonModule } from '@angular/common';
import { Component,OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { first } from 'rxjs';
import { RegisterUserService } from '../../Service/register-user.service';
import { User } from '../../model/user';
import { Router, RouterLink } from '@angular/router';


@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  registerForm:any;
  selectedFile: File | null = null;
  constructor(private fb:FormBuilder,private registerUserService:RegisterUserService,private router:Router) { }
  ngOnInit() {
    this.registerForm = this.fb.group({
      firstName: this.fb.control('',Validators.required),
      lastName: this.fb.control('',Validators.required),
      gender: this.fb.control('Male',Validators.required),
      birthday: this.fb.control('',Validators.required),
      location: this.fb.control('',Validators.required),
      email: this.fb.control('',Validators.compose([Validators.required,Validators.email])),
      username: this.fb.control('',Validators.required),
      password: this.fb.control('',Validators.required),
      bio: this.fb.control('')
    });
  }

  UploadFile(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      this.selectedFile = fileInput.files[0];
    }
  }
  RegisterUser() {
    const formData = this.CreateForm();
    this.registerUserService.RegisterUser(formData).subscribe({
      next: (data: User) => {
        console.log('User registered:', data);
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Error registering user:', err);
    }}); 
  }
  CreateForm() {
    let formData = new FormData();
    formData.append('firstName', this.registerForm.get('firstName')?.value);
    formData.append('lastName', this.registerForm.get('lastName')?.value);
    formData.append('gender', this.registerForm.get('gender')?.value);
    formData.append('birthday', this.registerForm.get('birthday')?.value);
    formData.append('location', this.registerForm.get('location')?.value);
    formData.append('email', this.registerForm.get('email')?.value);
    formData.append('username', this.registerForm.get('username')?.value);
    formData.append('password', this.registerForm.get('password')?.value);

    if (this.selectedFile) {
      formData.append('picture', this.selectedFile);
    }
    formData.forEach((value, key) => {
      console.log(`${key}: ${value}`);
    });
    return formData;
  }
}
