import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent {
  @Input() show = false;
  @Input() type = 'info';
  @Input() message = '';
  @Output() dismissed = new EventEmitter<void>();

  onDismiss() {
    this.dismissed.emit();
  }
}
