import { CommonModule } from '@angular/common';
import { Component, input } from '@angular/core';

@Component({
  selector: 'app-section-shell',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './section-shell.component.html',
  styleUrl: './section-shell.component.css'
})
export class SectionShellComponent {
  readonly eyebrow = input<string>('');
  readonly title = input.required<string>();
  readonly description = input<string>('');
}
