using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    // ���� ���� ������

    // �̵��� ü���ǿ� ���� ����
    // (ü����, ���� ��ġ, �� ��ġ, �̵� ����)
    public (Chessman chessman, (int x, int y) oldPosition, (int x, int y) newPosition, bool isMoved) movedChessman;

    // ���� ü���ǿ� ���� ���� (ŷ�� ������� ���� ���� ���� ������ ����)
    // (ü����, ��ġ)
    public (Chessman chessman, (int x, int y) Position) capturedChessman;

    // ���� ���� EnPassant �̵� ���� (�ؾ� ���� ���) 
    public (int x, int y) EnPassantStatus;

    // ���θ�� �̵��� ���� ����
    // (���θ�� ����, �±޵� ü����)
    public (bool wasPromotion, Chessman promotedChessman) PromotionMove;

    // ĳ���� �̵��� ���� ����
    // (ĳ���� ���� ����, ŷ ���̵� ����)
    public (bool wasCastling, bool isKingSide) CastlingMove;

    // ���� ������ Ʈ�� ����
    public int depth;

    // ���� �������� �����ϴ� �޼���
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
