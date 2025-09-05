import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent {
  @Input() title = '';
  @Input() show = false;
  @Input() disableBackdropClose = false;

  @Output() closed = new EventEmitter<void>();

  onBackdropClick(): void {
    if (this.disableBackdropClose) { return; }
    this.close();
  }

  close(): void {
    this.closed.emit();
  }
}


