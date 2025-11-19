export type SwipeCallback = (direction: 'left' | 'right') => void;
export type SwipeProgressCallback = (progress: number) => void;

export interface SwipeHandlerOptions {
  element: HTMLElement;
  onSwipe: SwipeCallback;
  onSwipeProgress?: SwipeProgressCallback;
  threshold?: number;
  maxRotation?: number;
}

export class SwipeHandler {
  private element: HTMLElement;
  private onSwipe: SwipeCallback;
  private onSwipeProgress?: SwipeProgressCallback;
  private threshold: number;
  private maxRotation: number;

  private startX: number = 0;
  private startY: number = 0;
  private currentX: number = 0;
  private isDragging: boolean = false;

  constructor(options: SwipeHandlerOptions) {
    this.element = options.element;
    this.onSwipe = options.onSwipe;
    this.onSwipeProgress = options.onSwipeProgress;
    this.threshold = options.threshold ?? 100;
    this.maxRotation = options.maxRotation ?? 15;

    this.bindEvents();
  }

  private bindEvents(): void {
    // Mouse events
    this.element.addEventListener('mousedown', this.handleStart.bind(this));
    document.addEventListener('mousemove', this.handleMove.bind(this));
    document.addEventListener('mouseup', this.handleEnd.bind(this));

    // Touch events
    this.element.addEventListener('touchstart', this.handleTouchStart.bind(this));
    document.addEventListener('touchmove', this.handleTouchMove.bind(this));
    document.addEventListener('touchend', this.handleEnd.bind(this));
  }

  private handleStart(e: MouseEvent): void {
    this.isDragging = true;
    this.startX = e.clientX;
    this.startY = e.clientY;
    this.currentX = 0;
    this.element.style.transition = 'none';
  }

  private handleTouchStart(e: TouchEvent): void {
    if (e.touches.length === 1) {
      this.isDragging = true;
      this.startX = e.touches[0].clientX;
      this.startY = e.touches[0].clientY;
      this.currentX = 0;
      this.element.style.transition = 'none';
    }
  }

  private handleMove(e: MouseEvent): void {
    if (!this.isDragging) return;

    this.currentX = e.clientX - this.startX;
    this.updateCardPosition();
  }

  private handleTouchMove(e: TouchEvent): void {
    if (!this.isDragging || e.touches.length !== 1) return;

    this.currentX = e.touches[0].clientX - this.startX;
    this.updateCardPosition();
  }

  private updateCardPosition(): void {
    const progress = Math.min(Math.abs(this.currentX) / this.threshold, 1);
    const rotation = (this.currentX / this.threshold) * this.maxRotation;

    this.element.style.transform = `translateX(${this.currentX}px) rotate(${rotation}deg)`;

    // Progress callback
    if (this.onSwipeProgress) {
      const signedProgress = this.currentX < 0 ? -progress : progress;
      this.onSwipeProgress(signedProgress);
    }
  }

  private handleEnd(): void {
    if (!this.isDragging) return;

    this.isDragging = false;
    this.element.style.transition = 'transform 0.3s ease';

    if (Math.abs(this.currentX) >= this.threshold) {
      // Swipe tamamlandı
      const direction = this.currentX < 0 ? 'left' : 'right';

      // Animasyon için kartı dışarı kaydır
      const exitX = this.currentX < 0 ? -500 : 500;
      this.element.style.transform = `translateX(${exitX}px) rotate(${this.currentX < 0 ? -30 : 30}deg)`;

      // Callback'i çağır
      setTimeout(() => {
        this.onSwipe(direction);
        this.resetPosition();
      }, 300);
    } else {
      // Swipe iptal - kartı geri getir
      this.resetPosition();
    }

    // Progress'i sıfırla
    if (this.onSwipeProgress) {
      this.onSwipeProgress(0);
    }
  }

  private resetPosition(): void {
    this.element.style.transform = 'translateX(0) rotate(0)';
  }

  // Programatik swipe
  public triggerSwipe(direction: 'left' | 'right'): void {
    const exitX = direction === 'left' ? -500 : 500;
    this.element.style.transition = 'transform 0.3s ease';
    this.element.style.transform = `translateX(${exitX}px) rotate(${direction === 'left' ? -30 : 30}deg)`;

    setTimeout(() => {
      this.onSwipe(direction);
      this.resetPosition();
    }, 300);
  }

  // Cleanup
  public destroy(): void {
    this.element.removeEventListener('mousedown', this.handleStart.bind(this));
    document.removeEventListener('mousemove', this.handleMove.bind(this));
    document.removeEventListener('mouseup', this.handleEnd.bind(this));

    this.element.removeEventListener('touchstart', this.handleTouchStart.bind(this));
    document.removeEventListener('touchmove', this.handleTouchMove.bind(this));
    document.removeEventListener('touchend', this.handleEnd.bind(this));
  }
}
