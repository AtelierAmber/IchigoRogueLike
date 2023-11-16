using System;
using System.Collections.Generic;
using SadConsole;


namespace Ichigo.Engine
{
  public readonly record struct Message(ColoredString Text, int Count);

  public class MessageAddedEventArgs : EventArgs
  {
    public Message Message { get; }

    public MessageAddedEventArgs(Message message)
    {
      Message = message;
    }
  }

  public static class MessageColors
  {
    /// <summary>
    /// Initial welcome text printed on dungeon entrance.
    /// </summary>
    public static readonly ColoredGlyphAndEffect WelcomeTextAppearance = new()
    {
      Foreground = new(0x20, 0xA0, 0xFF)
    };

    /// <summary>
    /// Text indicating the player attacked something.
    /// </summary>
    public static readonly ColoredGlyphAndEffect PlayerAtkAppearance = new()
    {
      Foreground = new(0xE0, 0xE0, 0xE0)
    };

    /// <summary>
    /// Text indicating an enemy attacked the player.
    /// </summary>
    public static readonly ColoredGlyphAndEffect EnemyAtkAppearance = new()
    {
      Foreground = new(0xFF, 0xC0, 0xC0)
    };

    /// <summary>
    /// Text indicating the player died.
    /// </summary>
    public static readonly ColoredGlyphAndEffect PlayerDiedAppearance = new()
    {
      Foreground = new(0xFF, 0x30, 0x30)
    };

    /// <summary>
    /// Text indicating an enemy died.
    /// </summary>
    public static readonly ColoredGlyphAndEffect EnemyDiedAppearance = new()
    {
      Foreground = new(0xFF, 0xA0, 0x30)
    };

    /// <summary>
    /// Text indicating the player tried to take an action which is not possible (ie. moving into a wall).
    /// </summary>
    public static readonly ColoredGlyphAndEffect ImpossibleActionAppearance = new()
    {
      Foreground = new(0x80, 0x80, 0x80)
    };

    /// <summary>
    /// Text indicating the player picked up an item.
    /// </summary>
    public static readonly ColoredGlyphAndEffect ItemPickedUpAppearance = new()
    {
      Foreground = new(0xFF, 0xFF, 0xFF)
    };

    /// <summary>
    /// Text indicating the player dropped an item.
    /// </summary>
    public static readonly ColoredGlyphAndEffect ItemDroppedAppearance = new()
    {
      Foreground = new(0xFF, 0xFF, 0xFF)
    };

    /// <summary>
    /// Text indicating the player recovered health.
    /// </summary>
    public static readonly ColoredGlyphAndEffect HealthRecoveredAppearance = new()
    {
      Foreground = new(0x0, 0xFF, 0x0)
    };

    /// <summary>
    /// Text indicating that a used item needs a target.
    /// </summary>
    public static readonly ColoredGlyphAndEffect NeedsTargetAppearance = new()
    {
      Foreground = new(0x3F, 0xFF, 0xFF)
    };

    /// <summary>
    /// Text indicating that a status effect was applied.
    /// </summary>
    public static readonly ColoredGlyphAndEffect StatusEffectAppliedAppearance = new()
    {
      Foreground = new(0x3F, 0xFF, 0xFF)
    };
  }

  /// <summary>
  /// A class that keeps a log of messages meant to be displayed to the user.
  /// </summary>
  public class MessageLog
  {
    

    private readonly List<Message> _messages;
    public IReadOnlyList<Message> Messages => _messages;

    public int MaxMessages { get; }

    public event EventHandler<MessageAddedEventArgs> MessageAdded;

    public MessageLog(int maxMessages)
    {
      _messages = new List<Message>();
      MaxMessages = maxMessages;
    }

    public void Add(ColoredString message)
    {
      if (_messages.Count == 0)
      {
        _messages.Add(new(message, 1));

      }
      else
      {
        if (_messages.Count >= MaxMessages)
          _messages.RemoveAt(0);

        var lastMessage = _messages[^1];

        // For now, we'll just blend messages with different colors but same content; but really we should take into account both
        if (lastMessage.Text.String == message.String)
          _messages[^1] = new(lastMessage.Text, lastMessage.Count + 1);
        else
          _messages.Add(new(message, 1));
      }

      MessageAdded?.Invoke(this, new MessageAddedEventArgs(_messages[^1]));
    }
  }
}
