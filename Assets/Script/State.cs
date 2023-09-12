using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    // 현재 상태 변수들

    // 이동된 체스맨에 관한 정보
    // (체스맨, 이전 위치, 새 위치, 이동 여부)
    public (Chessman chessman, (int x, int y) oldPosition, (int x, int y) newPosition, bool isMoved) movedChessman;

    // 잡힌 체스맨에 관한 정보 (킹을 잡았을때 게임 승패 유무 판정을 위해)
    // (체스맨, 위치)
    public (Chessman chessman, (int x, int y) Position) capturedChessman;

    // 현재 허용된 EnPassant 이동 상태 (해야 할지 고민) 
    public (int x, int y) EnPassantStatus;

    // 프로모션 이동에 관한 정보
    // (프로모션 여부, 승급된 체스맨)
    public (bool wasPromotion, Chessman promotedChessman) PromotionMove;

    // 캐슬링 이동에 관한 정보
    // (캐슬링 수행 여부, 킹 사이드 여부)
    public (bool wasCastling, bool isKingSide) CastlingMove;

    // 현재 상태의 트리 깊이
    public int depth;

    // 상태 변수들을 설정하는 메서드
    public void SetState((Chessman chessman, (int x, int y) oldPosition, (int x, int y) newPosition, bool isMoved) movedChessman,
                          (Chessman chessman, (int x, int y) Position) capturedChessman,
                          (int x, int y) EnPassantStatus,
                          (bool wasPromotion, Chessman promotedChessman) PromotionMove,
                          (bool wasCastling, bool isKingSide) CastlingMove,
                          int depth)
    {
        this.movedChessman = movedChessman;
        this.capturedChessman = capturedChessman;
        this.EnPassantStatus = EnPassantStatus;
        this.PromotionMove = PromotionMove;
        this.CastlingMove = CastlingMove;
        this.depth = depth;
    }
}
