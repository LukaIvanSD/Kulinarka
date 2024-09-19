import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../model/user';
@Injectable({
  providedIn: 'root'
})
export class RegisterUserService {

  constructor(private http:HttpClient) { }
  RegisterUser(newUser: FormData) {
     return this.http.post<User>('https://localhost:7289/Users', newUser);
  }
}
