import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, Router, RouterOutlet } from '@angular/router';
import { SessionService } from './Service/session-service.service';
import { User } from './model/user';
import { LoginModel } from './model/login';
import { StatusBarComponent } from './common/status-bar/status-bar.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule,RouterLink,RouterOutlet,StatusBarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
}
