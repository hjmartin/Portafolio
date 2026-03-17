import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

import { InfoCardComponent } from '../../../../shared/components/info-card/info-card.component';
import { SectionShellComponent } from '../../../../shared/components/section-shell/section-shell.component';
import { PillComponent } from '../../../../shared/ui/pill/pill.component';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [CommonModule, RouterLink, SectionShellComponent, InfoCardComponent, PillComponent],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent {
  protected readonly pillars = [
    'Core con auth, guards e interceptors',
    'Features separadas por dominio',
    'Shell publico y admin listos para crecer'
  ];
}
