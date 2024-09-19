import { Component,OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { LoginModel } from '../../model/login';
import { SessionService } from '../../Service/session-service.service';
import { User } from '../../model/user';
import { UserService } from '../../Service/user.service';
import { RouterLink,Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'] // Ispravljeno styleUrls
})
export class LoginComponent implements OnInit {
  user = {} as User;
  loginForm:any;

  constructor(private formBuilder: FormBuilder, private sessionService: SessionService,private userService:UserService,private router:Router) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({ // Inicijalizacija u ngOnInit metodi
      username: this.formBuilder.control(''),
      password: this.formBuilder.control(''),
      rememberMe: this.formBuilder.control(false)
    });
  }
  Login() {
    if (this.loginForm.valid) { // Provera da li je forma validna
      let loginModel = this.loginForm.value as LoginModel; // Korišćenje value umesto pojedinačnih get metoda
      console.log(loginModel);
      this.sessionService.Login(loginModel).subscribe((data: User) => {
        this.user = data;
        this.userService.setUser(this.user);
        this.router.navigate(['/']);
        console.log(data);
      });
    } else {
      console.error('Forma nije validna!');
    }
  }
}
