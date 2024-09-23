import { Component, DoCheck, OnInit } from '@angular/core';
import { RouterLink,Router } from '@angular/router';
import { SessionService } from '../../Service/session-service.service';
import { User } from '../../model/user';
import { CommonModule } from '@angular/common';
import { UserService } from '../../Service/user.service';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-status-bar',
  standalone: true,
  imports: [RouterLink,CommonModule],
  templateUrl: './status-bar.component.html',
  styleUrl: './status-bar.component.css'
})
export class StatusBarComponent implements OnInit,DoCheck {
  user:User | null=null;
  showStatusBar:boolean=false;
  imgSrc:string | null = null;
  private checkSessionInterval: Subscription | null = null;

  constructor(private sessionService:SessionService,private router : Router,private userService:UserService) {}

  ngOnInit(): void {
    this.GetSession();
    this.userService.user$.subscribe(user => {
      this.user = user;
    });
    this.checkSessionInterval = interval(10 * 60 * 1001)
    .subscribe(() => this.GetSession());
  }
  ngDoCheck(): void {
    let currenturl=this.router.url;
    if(currenturl=='/login' || currenturl=='/register'){
      this.showStatusBar=false
    }else{
      this.showStatusBar=true;
    }
  }

  Login(){
    this.router.navigate(['/login']);
  }

  Logout(){
    return this.sessionService.Logout().subscribe(
      (data:any)=>{
        this.userService.clearUser();
        this.router.navigate(['/']);
      }
    );
  }

  GetSession(){
    this.sessionService.GetSession().subscribe({
      next: (data: User) => {
        this.user = data;
        this.userService.setUser(this.user);
        console.log('User data:', this.user);
        if (this.user.picture) {
          this.imgSrc = `data:image/jpeg;base64,${this.user.picture}`;
        }
      },
      error: (err) => {
        console.error('Get session error:', err); // Obrada greške pri dobijanju sesije
      }
    });
  }
  Register(){
    this.router.navigate(['/register']);
  }
  ShowProfile(){
    this.router.navigate(['/profile']);
  }
  Home(){
    this.router.navigate(['/']);
  }
  AddRecipe(){
    this.router.navigate(['/addRecipe']);
  }
  MyRecipes(){
    this.router.navigate(['/myRecipes'])
  }
}
